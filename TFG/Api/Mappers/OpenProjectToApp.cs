using Shared.DTOs.Projects;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.OpenProjectClient.Models.WorkPackages;

namespace TFG.Api.Mappers
{
	public static class OpenProjectToApp
	{
		public static ProjectTaskDto ToProjectTaskDto(this WorkPackage workPackage, bool completed = false)
		{
			ProjectTaskDto projectTaskDto = new() 
			{ 
				Name = workPackage.Subject,  PercentageDone = workPackage.PercentageDone ?? 0, IsClosed = completed
			};

			return projectTaskDto;
		}
	}
}
