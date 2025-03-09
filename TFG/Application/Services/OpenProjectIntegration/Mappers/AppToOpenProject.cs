using Shared.DTOs.Projects;
using TFG.Infrastructure.Data;

namespace TFG.Application.Services.OpenProjectIntegration.Mappers
{
	public static class AppToOpenProject
	{
		public static OpenProjectFilterBuilder ToOpenProjectFilterBuilder(this ProjectTaskQueryParameters parameters, ApplicationDbContext dbContext)
		{

			OpenProjectFilterBuilder filter = new();
			if (parameters.UserIds != null && parameters.UserIds.Length != 0)
			{
				string[] openProjectUserIds = [.. dbContext.Users.Where(u => parameters.UserIds.Contains(u.Id)).Select(u => u.OpenProjectId)];
				filter.AddFilter("user", "=", openProjectUserIds);
			}

			return filter;
		}
	}
}
