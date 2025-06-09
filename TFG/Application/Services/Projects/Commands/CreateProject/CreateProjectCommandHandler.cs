using MediatR;
using Microsoft.EntityFrameworkCore;
using NGitLab;
using NGitLab.Models;
using Shared.DTOs.Projects;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Application.Security;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.Memberships;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models;
using GitlabProject = NGitLab.Models.Project;
using OPProjectCreated = TFG.OpenProjectClient.Models.Projects.ProjectCreated;
using OPProjectCreation = TFG.OpenProjectClient.Models.Projects.ProjectCreation;
using User = TFG.Domain.Entities.User;
using Project = TFG.Domain.Entities.Project;
using System.Threading.Tasks;

namespace TFG.Application.Services.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler(IUserInfoAccessor userInfoAccessor,
									  ApplicationDbContext dbContext,
									  IGitLabClient gitLabClient,
									  ISonarQubeClient sonarQubeClient,
									  IOpenProjectClient openProjectClient,
									  ILogger<CreateProjectCommandHandler> logger) : IRequestHandler<CreateProjectCommand, ProjectDto>
{
	public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
	{
		IUserInfo userInfo = userInfoAccessor.UserInfo;
		request.UsersIds ??= new List<string>();

		var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo.UserId) ?? throw new NotFoundException("User does not exist");
		long? gitlabId = null;
		string? sonarQubeProjectKey = null;
		int? openprojectProjectId = null;
		IEnumerable<User> projectUsers = await dbContext.Users.Where(u => request.UsersIds.Any(id => id == u.Id)).ToListAsync();
		try
		{
			var gitlabProject = await CreateAndConfigureGitlabProject(request, user, projectUsers);
			gitlabId = gitlabProject.Id;
			sonarQubeProjectKey = request.Name.ToLowerInvariant().Replace(" ", "_");
			string sonarQubeRepositoryIdentifier = gitlabProject.Id.ToString();
			BoundedProject sonarQubeProject = await CreateAndConfigureSonarQubeProject(request, sonarQubeProjectKey, sonarQubeRepositoryIdentifier, projectUsers);
			OPProjectCreated opProjectCreated = await CreateAndConfigureOpenProjectProject(request, projectUsers);
			openprojectProjectId = opProjectCreated.Id;
			Project createdProject = await CreateProjectInDatabase(request, projectUsers, gitlabProject, sonarQubeProjectKey, opProjectCreated.Id);

			return createdProject.ToProjectDto();
		}
		catch
		{
			await CancelCreation(gitlabId, sonarQubeProjectKey, openprojectProjectId);
			throw new Exception("An error occurred while creating the project. The project has been rolled back and no changes have been made.");
		}
	}

	private async Task<Project> CreateProjectInDatabase(CreateProjectCommand projectDto, IEnumerable<User> projectUsers, GitlabProject gitlabProjectResult, string sonarQubeProjectKey, int openProjectProjectId)
	{
		//Create the project in the database
		Project newProject = new()
		{
			Id = Guid.NewGuid(),
			Name = projectDto.Name,
			Description = projectDto.Description ?? string.Empty,
			SonarQubeProjectKey = sonarQubeProjectKey,
			GitlabId = gitlabProjectResult.Id.ToString(),
			OpenProjectId = openProjectProjectId,
			Users = projectUsers.ToList(),
			CreatedAt = DateTime.UtcNow
		};

		dbContext.Projects.Add(newProject);
		await dbContext.SaveChangesAsync();
		return newProject;
	}

	private async Task<GitlabProject> CreateAndConfigureGitlabProject(CreateProjectCommand request, User user, IEnumerable<User> projectUsers)
	{
		ProjectCreate gitLabProject = request.ToGitlabProjectCreate();
		GitlabProject createdProject = await gitLabClient.Projects.CreateAsync(gitLabProject);

		foreach (User projectUser in projectUsers)
		{
			ProjectMemberCreate member = new()
			{
				UserId = projectUser.GitlabId,
				AccessLevel = projectUser.Id != user.Id ? AccessLevel.Developer : AccessLevel.Owner
			};
			await gitLabClient.Members.AddMemberToProjectAsync(createdProject.Id, member);
		}

		return createdProject;
	}

	private async Task<BoundedProject> CreateAndConfigureSonarQubeProject(CreateProjectCommand projectDto, string projectKey, string repositoryIdentifier, IEnumerable<User> usersToInclude)
	{
		var dopSettings = await sonarQubeClient.DopTranslations.GetDopSettingsAsync();

		string gilabId = dopSettings.DopSettings.First(ds => ds.Type == "gitlab").Id;
		ProjectBinding projectBinding = new() { DevOpsPlatformSettingId = gilabId, ProjectKey = projectKey, ProjectName = projectDto.Name, RepositoryIdentifier = repositoryIdentifier, Monorepo = true };
		BoundedProject project = await sonarQubeClient.DopTranslations.BoundProjectAsync(projectBinding);

		foreach (var user in usersToInclude)
		{
			UserPermission userPermission = new() { Login = user.UserName!, ProjectKey = projectKey, Permission = PermissionType.Admin };
			await sonarQubeClient.Permissions.AddUserAsync(userPermission);
		}
		return project;
	}

	private async Task<OPProjectCreated> CreateAndConfigureOpenProjectProject(CreateProjectCommand projectDto, IEnumerable<User> users)
	{
		OPProjectCreation openProjectProjectCreation = new()
		{
			Name = projectDto.Name,
			Description = new()
			{
				Raw = projectDto.Description ?? string.Empty,
				Format = "markdown"
			},
			Identifier = projectDto.Name.ToLowerInvariant().Replace(" ", "_"),
			Public = true,
			Active = true,
		};
		OPProjectCreated opProjectCreated = await openProjectClient.Projects.CreateAsync(openProjectProjectCreation);
		foreach (int userOpenProjectId in users.Select(u => int.Parse(u.OpenProjectId)))
		{
			MembershipCreation membershipCreation = new()
			{
				Links = MembershipCreationLinksBuilder.Build(userOpenProjectId, [6], opProjectCreated.Id)
			};
			await openProjectClient.Memberships.CreateAsync(membershipCreation);
		}
		return opProjectCreated;
	}

	private async Task CancelCreation(long? gitlabId, string? sonarQubeKey, int? openProjectId)
	{
		List<Task<bool>> tasks = new()
		{
			TryDeleteGitlabProject(gitlabId),
			TryDeleteSonarQubeProject(sonarQubeKey),
			TryDeleteOpenprojectProject(openProjectId)
		};

		await Task.WhenAll(tasks);
	}

	private async Task<bool> TryDeleteGitlabProject(long? projectId)
	{
		if (projectId is null) return false;

		try
		{
			ProjectId gitlabId = projectId.Value;
			await gitLabClient.Projects.DeleteAsync(gitlabId);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private async Task<bool> TryDeleteSonarQubeProject(string? projectKey)
	{
		if (string.IsNullOrEmpty(projectKey)) return false;
		try
		{
			await sonarQubeClient.Projects.DeleteAsync(projectKey);
			return true;
		}
		catch
		{
			return false;
		}
	}

	private async Task<bool> TryDeleteOpenprojectProject(int? projectId)
	{
		if (projectId is null) return false;
		try
		{
			await openProjectClient.Projects.DeleteAsync(projectId.Value);
			return true;
		}
		catch
		{
			return false;
		}
	}
}
