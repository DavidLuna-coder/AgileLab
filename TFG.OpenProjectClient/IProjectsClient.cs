using TFG.OpenProjectClient.Models.Projects;

namespace TFG.OpenProjectClient
{
	public interface IProjectsClient
	{
		Task<ProjectCreated> CreateAsync(ProjectCreation projectCreation);
		Task DeleteAsync(int projectId);
	}
}
