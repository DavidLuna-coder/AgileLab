using MediatR;
using Microsoft.EntityFrameworkCore;
using NGitLab;
using NGitLab.Models;
using Shared.DTOs.Projects;
using System.Threading.Tasks;
using TFG.Api.Exeptions;
using TFG.Api.Mappers;
using TFG.Domain.Entities;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.SonarQubeClient;
using Project = TFG.Domain.Entities.Project;
using User = TFG.Domain.Entities.User;

namespace TFG.Application.Services.Projects.Commands.UpdateProject;

public class UpdateProjectCommand : IRequest<ProjectDto>
{
	public Guid ProjectId { get; set; }
	public string Name { get; set; }
	public string? Description { get; set; }
	public List<string>? UsersIds { get; set; }
}

public class UpdateProjectCommandHandler(ApplicationDbContext context,
										 IOpenProjectClient openProjectClient,
										 IGitLabClient gitLabClient,
										 ISonarQubeClient sonarQubeClient,
										 ILogger<UpdateProjectCommandHandler> logger) : IRequestHandler<UpdateProjectCommand,ProjectDto>
{
	public async Task<ProjectDto> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
	{
		var existingProject = await context.Projects
			.Include(p => p.Users)
			.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken) ?? throw new NotFoundException($"El proyecto con id {request.ProjectId} no existe");

		var requestUserIds = request.UsersIds ?? new List<string>();
		var projectUserIds = existingProject.Users.Select(u => u.Id).ToList();

		var usersToAddIds = requestUserIds.Except(projectUserIds).ToList();
		var usersToRemoveIds = projectUserIds.Except(requestUserIds).ToList();

		var allUserIds = usersToAddIds.Concat(usersToRemoveIds).ToList();
		var allUsers = await context.Users.Where(u => allUserIds.Contains(u.Id)).ToListAsync();
		var usersToAdd = allUsers.Where(u => usersToAddIds.Contains(u.Id)).ToList();
		var usersToRemove = allUsers.Where(u => usersToRemoveIds.Contains(u.Id)).ToList();

		// Add users
		foreach (var user in usersToAdd)
		{
			existingProject.Users.Add(user);
		}

		// Remove users
		foreach (var user in usersToRemove)
		{
			existingProject.Users.Remove(user);
		}

		existingProject.Name = request.Name;
		existingProject.Description = request.Description ?? string.Empty;

		await UpdateProjectInGitlab(request, existingProject, usersToAdd, usersToRemove);

		await context.SaveChangesAsync(cancellationToken);

		return existingProject.ToProjectDto();
	}

	private async Task UpdateProjectInGitlab(UpdateProjectCommand request, Project existingProject, List<User> usersToAdd, List<User> usersToRemove)
	{
		ProjectUpdate projectUpdate = request.ToGitlabProjectUpdate();
		await gitLabClient.Projects.UpdateAsync(existingProject.GitlabId, projectUpdate);

		foreach (var user in usersToRemove)
		{
			try
			{
				await gitLabClient.Members.RemoveMemberFromProjectAsync(existingProject.GitlabId, long.Parse(user.GitlabId));
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex,
								  "No se pudo eliminar al usuario {UserName} del proyecto {ProjectName} en GitLab. Puede que no exista o que ya haya sido eliminado.",
								  user.UserName,
								  existingProject.Name);
				continue; // Continue even if we can't remove a user
			}
		}

		foreach (var user in usersToAdd)
		{
			ProjectMemberCreate member = new()
			{
				UserId = user.GitlabId,
				AccessLevel = AccessLevel.Developer
			};
			try
			{
				await gitLabClient.Members.AddMemberToProjectAsync(existingProject.GitlabId, member);
			}
			catch (Exception ex)
			{
				logger.LogWarning(ex,
				  "No se pudo Agregar al usuario {UserName} del proyecto {ProjectName} en GitLab. Puede que no exista o que ya haya sido eliminado.",
				  user.UserName,
				  existingProject.Name);
				continue; // Continue even if we can't add a user
			}
		}
	}
}
