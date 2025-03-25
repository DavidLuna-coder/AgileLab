using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient
{
    public interface IProjectsClient
    {
        Task<ProjectCreated> CreateAsync(ProjectCreation project);
        Task DeleteAsync(string projectKey);
	}
}
