using TFG.SonarQubeClient.Models;
using TFG.SonarQubeClient.Models.Metrics;

namespace TFG.SonarQubeClient
{
    public interface IProjectsClient
    {
        Task<ProjectCreated> CreateAsync(ProjectCreation project);
        Task DeleteAsync(string projectKey);
        Task<SonarMetricsResponse> GetMetrics(SonarMetricsRequest request);
        Task<SonarComponentsTree> GetComponentsTree(GetComponentsRequest request);
	}
}
