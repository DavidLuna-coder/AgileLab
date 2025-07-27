using NGitLab.Models;
using Shared.DTOs.Projects;
using TFG.Application.Services.Projects.Commands.CreateProject;
using TFG.Application.Services.Projects.Commands.UpdateProject;

namespace TFG.Api.Mappers
{
	public static class AppDomainToGitlabDomainMapper
	{
		public static ProjectCreate ToGitlabProjectCreate(this CreateProjectDto createProjectDto)
		{
			return new ProjectCreate()
			{
				Description = createProjectDto.Description,
				Name = createProjectDto.Name,
				VisibilityLevel = VisibilityLevel.Public,
				TemplateName = createProjectDto.Template,
				MergeRequestsAccessLevel = "enabled",
			};
		}

		public static ProjectCreate ToGitlabProjectCreate(this CreateProjectCommand createProjectDto)
		{
			return new ProjectCreate()
			{
				Description = createProjectDto.Description,
				Name = createProjectDto.Name,
				VisibilityLevel = VisibilityLevel.Public,
				TemplateName = createProjectDto.Template,
				MergeRequestsAccessLevel = "enabled",
			};
		}

		public static ProjectUpdate ToGitlabProjectUpdate(this UpdateProjectCommand updateProjectCommand)
		{
			return new ProjectUpdate()
			{
				Description = updateProjectCommand.Description,
				Name = updateProjectCommand.Name,
			};
		}
	}
}
