using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.DTOs.Projects;
using TFG.Api.Mappers;
using TFG.Application.Dtos;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Interfaces.Projects;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;

namespace TFG.Application.Services.Projects
{
	public class ProjectService(IGitlabApiIntegration gitlabApiIntegration, ApplicationDbContext dbContext, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor) : IProjectService
	{
		private readonly IGitlabApiIntegration _gitlabApiIntegration = gitlabApiIntegration;
		private readonly ApplicationDbContext _dbContext = dbContext;
		private readonly UserManager<User> _userManager = userManager;
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
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
			var result = await _gitlabApiIntegration.CreateProject(projectDto, int.Parse(user.GitlabId));
			if (!result.Success)
			{
				return new Result<Project>(result.Errors);
			}


			//Create the project in the database
			Project newProject = projectDto.ToProject();
			IEnumerable<User> projectUsers = _userManager.Users.Where(u => projectDto.UsersIds.Any(id => id == u.Id)).ToList();
			newProject.Users = projectUsers.ToList();
			newProject.CreatedAt = DateTime.UtcNow;
			_dbContext.Projects.Add(newProject);
			await _dbContext.SaveChangesAsync();
			return new Result<Project>(newProject);
		}
	}

}
