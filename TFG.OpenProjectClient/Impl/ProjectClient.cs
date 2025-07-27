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

		public Task DeleteAsync(int projectId)
		{
			return httpClient.DeleteAsync($"projects/{projectId}");
		}

		public Task UpdateAsync(ProjectUpdate projectUpdate)
		{
			return httpClient.PatchAsync($"projects/{projectUpdate.Id}", projectUpdate);
		}
	}
}
