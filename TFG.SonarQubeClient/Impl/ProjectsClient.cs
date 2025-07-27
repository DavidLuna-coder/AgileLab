using System.Runtime.Serialization;
using System.Text.Json;
using TFG.SonarQubeClient.Models;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.SonarQubeClient.Impl
{
	public class ProjectsClient(ISonarHttpClient client) : IProjectsClient
	{
		public Task<ProjectCreated> CreateAsync(ProjectCreation project)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(string projectKey)
		{
			return client.PostAsync($"projects/delete?project={projectKey}");
		}

		public async Task<SonarComponentsTree> GetComponentsTree(GetComponentsRequest request)
		{
			var response = await client.GetAsync(request.BuildRequestUri());
			var content = await response.Content.ReadAsStringAsync();
			SonarComponentsTree components = JsonSerializer.Deserialize<SonarComponentsTree>(content) ?? throw new SerializationException();

			return components;
		}

		public async Task<SonarMetricsResponse> GetMetrics(SonarMetricsRequest request)
		{
			var response = await client.GetAsync(request.BuildRequestUri());
			var content = await response.Content.ReadAsStringAsync();
			SonarMetricsResponse metrics = JsonSerializer.Deserialize<SonarMetricsResponse>(content) ?? throw new SerializationException();

			return metrics;
		}

		public Task UpdateKeyAsync(UpdateProjectKey updateProject)
		{
			return client.PostAsync($"projects/update_key?from={updateProject.From}&to={updateProject.To}");
		}
	}
}
