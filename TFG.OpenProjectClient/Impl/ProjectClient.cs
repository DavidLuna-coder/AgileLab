using System.Text.Json;
using TFG.OpenProjectClient.Models.Projects;

namespace TFG.OpenProjectClient.Impl
{
	public class ProjectClient(IOpenProjectHttpClient httpClient) : IProjectsClient
	{
		public async Task<ProjectCreated> CreateAsync(ProjectCreation projectCreation)
		{
			var response = await httpClient.PostAsync("projects", projectCreation);
			string responseBody = await response.Content.ReadAsStringAsync();
			ProjectCreated projectCreated = JsonSerializer.Deserialize<ProjectCreated>(responseBody)!;
			return projectCreated;
		}

		public async Task DeleteAsync(int projectId)
		{
			await httpClient.DeleteAsync($"projects/{projectId}");
		}
	}
}
