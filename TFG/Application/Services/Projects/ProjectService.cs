using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects;
using TFG.Api.Mappers;
using TFG.Application.Dtos;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Interfaces.Projects;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Application.Services.Dtos;
using TFG.Application.Services.GitlabIntegration.Dtos;
using TFG.Application.Services.GitlabIntegration.Enums;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Application.Services.OpenProjectIntegration.Mappers;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;

namespace TFG.Application.Services.Projects
{
	public class ProjectService(IGitlabApiIntegration gitlabApiIntegration, ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IOpenProjectApiIntegration openProjectApiIntegration, ISonarQubeApiIntegration sonarQubeApiIntegration, ILogger<ProjectService> logger) : IProjectService
	{
		private readonly IGitlabApiIntegration _gitlabApiIntegration = gitlabApiIntegration;
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly UserManager<User> _userManager = userManager;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
		private readonly IOpenProjectApiIntegration _openProjectApiIntegration = openProjectApiIntegration;
		public readonly ISonarQubeApiIntegration _sonarQubeApiIntegration = sonarQubeApiIntegration;
		public async Task<Result<Project>> CreateProject(CreateProjectDto projectDto)
		{
			//Get the user id from the token
			UserInfo userInfo = _httpContextAccessor.HttpContext!.Items["UserInfo"] as UserInfo ?? new();
			projectDto.UsersIds ??= [];

			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo.UserId);
			if (user == null) return new Result<Project>(["User not found"]);


			IEnumerable<User> projectUsers = _userManager.Users.Where(u => projectDto.UsersIds.Any(id => id == u.Id)).ToList();
			var gitlabProjectResult = await CreateAndConfigureGitlabProject(projectDto, user, projectUsers);
			if (!gitlabProjectResult.Success) return new Result<Project>(gitlabProjectResult.Errors);

			//Create Project in SonarQube
			string sonarQubeProjectKey = projectDto.Name.ToLowerInvariant().Replace(" ", "_");
			string sonarQubeRepositoryIdentifier = gitlabProjectResult.Value.Id.ToString();
			var sonarQubeProjectResult = await CreateAndConfigureSonarQubeProject(projectDto, sonarQubeProjectKey, sonarQubeRepositoryIdentifier, projectUsers);
			if (!sonarQubeProjectResult.Success) return new Result<Project>(sonarQubeProjectResult.Errors);

			var openProjectProjectResult = await CreateAndConfigureOpenProjectProject(projectDto, projectUsers);
			if (!openProjectProjectResult.Success) return new Result<Project>(openProjectProjectResult.Errors);

			return await CreateProjectInDatabase(projectDto, projectUsers, gitlabProjectResult, sonarQubeProjectKey, openProjectProjectResult);
		}

		private async Task<Result<Project>> CreateProjectInDatabase(CreateProjectDto projectDto, IEnumerable<User> projectUsers, Result<GitlabCreateProjectResponseDto> gitlabProjectResult, string sonarQubeProjectKey, Result<int> openProjectProjectResult)
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
			var gitlabDeletionResult = await _gitlabApiIntegration.DeleteProject(projectToDelete.GitlabId);
			//if (!gitlabDeletionResult.Success) return new Result<bool>(gitlabDeletionResult.Errors);

			//Delete the project in OpenProject
			var openProjectDeletionResult = await _openProjectApiIntegration.DeleteProject(projectToDelete.OpenProjectId);
			//if (!openProjectDeletionResult.Success) return new Result<bool>(openProjectDeletionResult.Errors);
			SonarQubeDeleteProjectDto sonarQubeDeleteProjectDto = new()
			{
				Project = projectToDelete.SonarQubeProjectKey
			};
			var sonarQubeDeletionResult = await _sonarQubeApiIntegration.DeleteProject(sonarQubeDeleteProjectDto);
			//Delete the project in the database
			_dbContext.Projects.Remove(projectToDelete);
			await _dbContext.SaveChangesAsync();
			return true;
		}

		public async Task<List<ProjectTaskDto>> GetProjectTasks(Guid projectId, ProjectTaskQueryParameters requestDto)
		{
			OpenProjectFilterBuilder openProjectFilterBuilder = requestDto.ToOpenProjectFilterBuilder(_dbContext);
			int openProjectProjectId = await _dbContext.Projects.Where(p => p.Id == projectId).Select(p => p.OpenProjectId).FirstOrDefaultAsync();
			if (openProjectProjectId == default)
			{
				throw new ArgumentException("The project does not exist");
			}

			Result<OpenProjectWorkPackage[]> result = await _openProjectApiIntegration.GetWorkPackages(openProjectProjectId, openProjectFilterBuilder);
			if (!result.Success)
			{
				logger.LogError("Get Project Task Errors: {errors}", string.Join(',', result.Errors));
				return [];
			}

			return result.Value.Select(wp => wp.ToProjectTaskDto()).ToList();
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

			//TODO ADD USERS TO OPENPROJECT
			if (!openProjectCreateProjectResult.Success) return new Result<int>(openProjectCreateProjectResult.Errors);

			return openProjectCreateProjectResult;
		}

		private async Task<Result<SonarQubeCreateProjectResponseDto>> CreateAndConfigureSonarQubeProject(CreateProjectDto projectDto, string projectKey, string repositoryIdentifier, IEnumerable<User> usersToInclude)
		{
			string gilabId = (await _sonarQubeApiIntegration.GetDopSettings()).Value.DopSettings.Where(ds => ds.Type == "gitlab").First().Id;
			SonarQubeCreateProjectRequestDto sonarQubeCreateProjectRequestDto = new()
			{
				DevOpsPlatformSettingId = gilabId,
				ProjectKey = projectKey,
				ProjectName = projectDto.Name,
				RepositoryIdentifier = repositoryIdentifier,
			};
			Result<SonarQubeCreateProjectResponseDto> sonarQubeCreateProjectResult = await _sonarQubeApiIntegration.CreateProject(sonarQubeCreateProjectRequestDto);
			if (!sonarQubeCreateProjectResult.Success) return new Result<SonarQubeCreateProjectResponseDto>(sonarQubeCreateProjectResult.Errors);

			foreach (var user in usersToInclude)
			{
				SonarQubeCreateUserPermissionDto sonarQubeCreateUserPermissionDto = new()
				{
					ProjectKey = projectKey,
					Login = user.UserName!,
					Permission = SonarQubeProjectPermissionConstants.Admin
				};
				var sonarQubeUserCreationResult = await _sonarQubeApiIntegration.CreateUserPermission(sonarQubeCreateUserPermissionDto);
				if (!sonarQubeUserCreationResult.Success) return new Result<SonarQubeCreateProjectResponseDto>(sonarQubeUserCreationResult.Errors);
			}
			return sonarQubeCreateProjectResult;
		}

		private async Task<Result<GitlabCreateProjectResponseDto>> CreateAndConfigureGitlabProject(CreateProjectDto projectDto, User user, IEnumerable<User> projectUsers)
		{
			//Create the project in Gitlab
			var createdProjectResult = await _gitlabApiIntegration.CreateProject(projectDto, int.Parse(user.GitlabId));
			if (!createdProjectResult.Success)
			{
				return new Result<GitlabCreateProjectResponseDto>(createdProjectResult.Errors);
			}

			//Add the users to the project in Gitlab
			GitlabAddMembersToProjectDto gitlabAddMemberToProjectDto = new()
			{
				Id = createdProjectResult.Value.Id,
				UserId = string.Join(",", projectUsers.Where(u => u.GitlabId != user.GitlabId).Select(u => int.Parse(u.GitlabId))),
				AccessLevel = GitlabAcessLevel.Developer
			};

			return createdProjectResult;
		}
	}

}
