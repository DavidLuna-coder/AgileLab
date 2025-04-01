using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NGitLab;
using NGitLab.Models;
using Shared.DTOs.Projects;
using TFG.Api.Mappers;
using TFG.Application.Dtos;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Interfaces.Projects;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Application.Services.OpenProjectIntegration.Mappers;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using Project = TFG.Model.Entities.Project;
using GitlabProject = NGitLab.Models.Project;
using User = TFG.Model.Entities.User;
using TFG.SonarQubeClient;
using TFG.SonarQubeClient.Models;

namespace TFG.Application.Services.Projects
{
	public class ProjectService(ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IOpenProjectApiIntegration openProjectApiIntegration, IGitLabClient gitLabClient, ISonarQubeClient sonarClient, ILogger<ProjectService> logger) : IProjectService
	{
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly UserManager<User> _userManager = userManager;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		private readonly IOpenProjectApiIntegration _openProjectApiIntegration = openProjectApiIntegration;
		private readonly IGitLabClient _gitLabClient = gitLabClient;
		private readonly ISonarQubeClient _sonarQubeClient = sonarClient;

		public async Task<Result<Project>> CreateProject(CreateProjectDto projectDto)
		{
			//Get the user id from the token
			UserInfo userInfo = _httpContextAccessor.HttpContext!.Items["UserInfo"] as UserInfo ?? new();
			projectDto.UsersIds ??= [];

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo.UserId);
			if (user == null) return new Result<Project>(["User not found"]);


			IEnumerable<User> projectUsers = await _userManager.Users.Where(u => projectDto.UsersIds.Any(id => id == u.Id)).ToListAsync();
			var gitlabProjectResult = await CreateAndConfigureGitlabProject(projectDto, user, projectUsers);
			if (!gitlabProjectResult.Success) return new Result<Project>(gitlabProjectResult.Errors);

			//Create Project in SonarQube
			string sonarQubeProjectKey = projectDto.Name.ToLowerInvariant().Replace(" ", "_");
			string sonarQubeRepositoryIdentifier = gitlabProjectResult.Value.Id.ToString();
			await CreateAndConfigureSonarQubeProject(projectDto, sonarQubeProjectKey, sonarQubeRepositoryIdentifier, projectUsers);

			var openProjectProjectResult = await CreateAndConfigureOpenProjectProject(projectDto, projectUsers);
			if (!openProjectProjectResult.Success) return new Result<Project>(openProjectProjectResult.Errors);

			return await CreateProjectInDatabase(projectDto, projectUsers, gitlabProjectResult, sonarQubeProjectKey, openProjectProjectResult);
		}

		private async Task<Result<Project>> CreateProjectInDatabase(CreateProjectDto projectDto, IEnumerable<User> projectUsers, Result<GitlabProject> gitlabProjectResult, string sonarQubeProjectKey, Result<int> openProjectProjectResult)
		{
			//Create the project in the database
			Project newProject = projectDto.ToProject();
			newProject.SonarQubeProjectKey = sonarQubeProjectKey;
			newProject.GitlabId = gitlabProjectResult.Value.Id.ToString();
			newProject.OpenProjectId = openProjectProjectResult.Value;
			newProject.Users = projectUsers.ToList();
			newProject.CreatedAt = DateTime.UtcNow;
			_dbContext.Projects.Add(newProject);
			await _dbContext.SaveChangesAsync();
			return new Result<Project>(newProject);
		}

		public async Task<Result<bool>> DeleteProject(Guid id)
		{
			UserInfo userInfo = _httpContextAccessor.HttpContext!.Items["UserInfo"] as UserInfo ?? new();
			if (!userInfo.IsAdmin) return new Result<bool>(["Not enough permissions to delete de project"]);

			Project? projectToDelete = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
			if (projectToDelete is null) return new Result<bool>(["The project does not exist"]);

			//Delete the project in Gitlab
			await _gitLabClient.Projects.DeleteAsync(projectToDelete.GitlabId);

			//Delete the project in OpenProject
			var openProjectDeletionResult = await _openProjectApiIntegration.DeleteProject(projectToDelete.OpenProjectId);

			await _sonarQubeClient.Projects.DeleteAsync(projectToDelete.SonarQubeProjectKey);
			//Delete the project in the database
			_dbContext.Projects.Remove(projectToDelete);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<List<ProjectTaskDto>> GetProjectTasks(Guid projectId, ProjectTaskQueryParameters requestDto)
		{
			int openProjectProjectId = await _dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.OpenProjectId).FirstOrDefaultAsync();
			if (openProjectProjectId == default)
			{
				throw new ArgumentException("The project does not exist");
			}

			OpenProjectFilterBuilder openProjectFilterBuilder = requestDto.ToOpenProjectFilterBuilder(_dbContext);
			Result<OpenProjectCollection<OpenProjectStatus>> statusesResult = await _openProjectApiIntegration.GetStatuses();
			if (!statusesResult.Success)
			{
				logger.LogError("Get Project Task Errors: {Errors}", string.Join(',', statusesResult.Errors));
				return [];
			}


			IEnumerable<OpenProjectStatus> closedStatuses = statusesResult.Value.Embedded.Elements.Where(s => s.IsClosed);
			IEnumerable<OpenProjectStatus> openStatuses = statusesResult.Value.Embedded.Elements.Where(s => !s.IsClosed);
			Result<OpenProjectWorkPackage[]> closedTasks = new(new List<OpenProjectWorkPackage>().ToArray());
			if (closedStatuses.Any())
			{
				openProjectFilterBuilder.AddFilter("status", "=", [.. closedStatuses.Select(s => s.Id.ToString())]);
				closedTasks = await _openProjectApiIntegration.GetWorkPackages(openProjectProjectId, openProjectFilterBuilder);
			}
			openProjectFilterBuilder.AddFilter("status", "=", [.. openStatuses.Select(s => s.Id.ToString())]);
			Result<OpenProjectWorkPackage[]> openTasks = await _openProjectApiIntegration.GetWorkPackages(openProjectProjectId, openProjectFilterBuilder);

			if (!closedTasks.Success || !openTasks.Success)
			{
				logger.LogError("Get Project Task Errors: {Errors}", string.Join(',', closedTasks.Errors));
				return [];
			}

			var result = new List<ProjectTaskDto>();
			result.AddRange(closedTasks.Value.Select(t => t.ToProjectTaskDto(true)));
			result.AddRange(openTasks.Value.Select(t => t.ToProjectTaskDto(false)));
			return result.ToList();
		}
		private async Task<Result<int>> CreateAndConfigureOpenProjectProject(CreateProjectDto projectDto, IEnumerable<User> users)
		{
			//Create the project in OpenProject
			OpenProjectCreateProjectDto openProjectCreateProjectDto = new()
			{
				Name = projectDto.Name
			};
			Result<int> openProjectCreateProjectResult = await _openProjectApiIntegration.CreateProject(openProjectCreateProjectDto);

			foreach (var user in users)
			{
				await _openProjectApiIntegration.CreateMembership(int.Parse(user.OpenProjectId), openProjectCreateProjectResult.Value, [6]);
			}

			if (!openProjectCreateProjectResult.Success) return new Result<int>(openProjectCreateProjectResult.Errors);

			return openProjectCreateProjectResult;
		}

		private async Task<BoundedProject> CreateAndConfigureSonarQubeProject(CreateProjectDto projectDto, string projectKey, string repositoryIdentifier, IEnumerable<User> usersToInclude)
		{
			var test = await _sonarQubeClient.DopTranslations.GetDopSettingsAsync();

			string gilabId = test.DopSettings.First(ds => ds.Type == "gitlab").Id;
			ProjectBinding projectBinding = new() { DevOpsPlatformSettingId = gilabId, ProjectKey = projectKey, ProjectName = projectDto.Name, RepositoryIdentifier = repositoryIdentifier, Monorepo = true };
			BoundedProject project = await _sonarQubeClient.DopTranslations.BoundProjectAsync(projectBinding);

			foreach (var user in usersToInclude)
			{
				UserPermission userPermission = new() { Login = user.UserName!, ProjectKey = projectKey, Permission = PermissionType.Admin };
				await _sonarQubeClient.Permissions.AddUserAsync(userPermission);
			}
			return project;
		}

		private async Task<Result<GitlabProject>> CreateAndConfigureGitlabProject(CreateProjectDto projectDto, User user, IEnumerable<User> projectUsers)
		{
			//Create the project in Gitlab
			ProjectCreate gitLabProject = projectDto.ToGitlabProjectCreate();
			GitlabProject createdProject = await _gitLabClient.Projects.CreateAsync(gitLabProject);

			foreach (User projectUser in projectUsers)
			{
				ProjectMemberCreate member = new()
				{
					UserId = projectUser.GitlabId,
					AccessLevel = projectUser.Id != user.Id ? AccessLevel.Developer : AccessLevel.Owner
				};
				await _gitLabClient.Members.AddMemberToProjectAsync(createdProject.Id, member);
			}

			return createdProject;
		}
	}

}
