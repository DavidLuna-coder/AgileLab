using Shared.DTOs.Projects;
using TFG.Domain.Entities;

namespace TFG.Api.Mappers
{
	public static class ApiContractToDomainMapper
	{
		public static Project ToProject(this CreateProjectDto createProjectDto)
		{
			return new Project
			{
				Id = Guid.NewGuid(),
				Name = createProjectDto.Name,
				Description = createProjectDto.Description ?? string.Empty,
			};
		}
		public static void ToProject(this UpdateProjectDto updateProjectDto, Project existingProject)
		{
			existingProject.Name = updateProjectDto.Name;
			existingProject.Description = updateProjectDto.Description ?? string.Empty;
		}

	}
}
