using NGitLab.Models;
using Shared.DTOs.Projects;

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
				VisibilityLevel = VisibilityLevel.Public
			};
		}
	}
}
