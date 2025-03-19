using TFG.SonarQubeClient.Models;

namespace TFG.SonarQubeClient.Impl
{
	public class ProjectsClient(ISonarHttpClient client) : IProjectsClient
	{
		public Task<ProjectCreated> CreateAsync(ProjectCreation project)
		{
			throw new NotImplementedException();
		}

		public Task DeleteAsync(ProjectDeletion projectKey)
		{
			return client.PostAsync($"projects/delete?project={projectKey}", projectKey);
		}
	}
}
