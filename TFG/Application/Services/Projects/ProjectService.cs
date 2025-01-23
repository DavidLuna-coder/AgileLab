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
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;

namespace TFG.Application.Services.Projects
{
	public class ProjectService(IGitlabApiIntegration gitlabApiIntegration, ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IOpenProjectApiIntegration openProjectApiIntegration, ISonarQubeApiIntegration sonarQubeApiIntegration) : IProjectService
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

			//Get the user from the database
			var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == userInfo.UserId);
			if (user == null)
			{
				return new Result<Project>(["User not found"]);
			}

			//Create the project in Gitlab
			Result<GitlabCreateProjectResponseDto> createdProjectResult = await _gitlabApiIntegration.CreateProject(projectDto, int.Parse(user.GitlabId));
			if (!createdProjectResult.Success)
			{
				return new Result<Project>(createdProjectResult.Errors);
			}
			IEnumerable<User> projectUsers = _userManager.Users.Where(u => projectDto.UsersIds.Any(id => id == u.Id)).ToList();

			//Add the users to the project in Gitlab
			GitlabAddMembersToProjectDto gitlabAddMemberToProjectDto = new()
			{
				Id = createdProjectResult.Value.Id,
				UserId = string.Join(",", projectUsers.Select(u => int.Parse(u.GitlabId))),
				AccessLevel = GitlabAcessLevel.Developer
			};
			var addUsersToProjectResult = await _gitlabApiIntegration.AddUsersToProject(gitlabAddMemberToProjectDto);
			if (!addUsersToProjectResult.Success) return new Result<Project>(addUsersToProjectResult.Errors);

			//Create Project in SonarQube
			string gilabId = (await _sonarQubeApiIntegration.GetDopSettings()).Value.DopSettings.Where(ds => ds.Type == "gitlab").First().Id;
			SonarQubeCreateProjectRequestDto sonarQubeCreateProjectRequestDto = new()
			{
				DevOpsPlatformSettingId = gilabId,
				ProjectKey = projectDto.Name,
				ProjectName = projectDto.Name,
				RepositoryIdentifier = createdProjectResult.Value.Id.ToString(),
			};
			var sonarQubeCreateProjectResult = await _sonarQubeApiIntegration.CreateProject(sonarQubeCreateProjectRequestDto);
			//TODO ADD USERS TO SONARQUBE


			//Create the project in OpenProject
			OpenProjectCreateProjectDto openProjectCreateProjectDto = new()
			{
				Name = projectDto.Name
			};
			Result<int> openProjectCreateProjectResult = await _openProjectApiIntegration.CreateProject(openProjectCreateProjectDto);
			//TODO ADD USERS TO OPENPROJECT
			//if (!openProjectCreateProjectResult.Success) return new Result<Project>(openProjectCreateProjectResult.Errors);

			//Create the project in the database
			Project newProject = projectDto.ToProject();
			newProject.GitlabId = createdProjectResult.Value.Id.ToString();
			newProject.OpenProjectId = openProjectCreateProjectResult.Value;
			newProject.Users = projectUsers.ToList();
			newProject.CreatedAt = DateTime.UtcNow;
			_dbContext.Projects.Add(newProject);
			await _dbContext.SaveChangesAsync();
			return new Result<Project>(newProject);
		}
		public async Task<Result<bool>> DeleteProject(Guid id)
		{
			//Validación de que el usuario puede borrar el proyecto
			UserInfo userInfo = _httpContextAccessor.HttpContext!.Items["UserInfo"] as UserInfo ?? new();
			//De momento solo los administradores pueden borrar los proyectos.
			if (!userInfo.IsAdmin) return new Result<bool>(["Not enough permissions to delete de project"]);

			Project? projectToDelete = await _dbContext.Projects.FirstOrDefaultAsync(p => p.Id == id);
			if (projectToDelete is null) return new Result<bool>(["The project does not exist"]);

			//Delete the project in Gitlab
			var gitlabDeletionResult = await _gitlabApiIntegration.DeleteProject(projectToDelete.GitlabId);
			//if (!gitlabDeletionResult.Success) return new Result<bool>(gitlabDeletionResult.Errors);

			//Delete the project in OpenProject
			var openProjectDeletionResult = await _openProjectApiIntegration.DeleteProject(projectToDelete.OpenProjectId);
			//if (!openProjectDeletionResult.Success) return new Result<bool>(openProjectDeletionResult.Errors);

			//Delete the project in the database
			_dbContext.Projects.Remove(projectToDelete);
			await _dbContext.SaveChangesAsync();
			return true;
		}
	}

}
