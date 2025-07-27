using TFG.OpenProjectClient.Models.Projects;

namespace TFG.OpenProjectClient
{
	public interface IProjectsClient
	{
		Task<ProjectCreated> CreateAsync(ProjectCreation projectCreation);
		Task UpdateAsync(ProjectUpdate projectUpdate);
		Task DeleteAsync(int projectId);
	}
}
