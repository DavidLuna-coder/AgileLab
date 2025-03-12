using Shared.DTOs.Projects;
using TFG.Application.Services.OpenProjectIntegration.Dtos;

namespace TFG.Application.Services.OpenProjectIntegration.Mappers
{
	public static class OpenProjectToApp
	{
		public static ProjectTaskDto ToProjectTaskDto(this OpenProjectWorkPackage workPackage, bool completed = false)
		{
			ProjectTaskDto projectTaskDto = new() 
			{ 
				Name = workPackage.Name,  PercentageDone = workPackage.PercentageDone  , IsClosed = completed
			};

			return projectTaskDto;
		}
	}
}
