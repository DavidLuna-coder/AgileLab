using Shared.DTOs.Projects;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.Projects
{
	public interface IProjectService
	{
		Task<Result<Project>> CreateProject(CreateProjectDto project);
		Task<Result<bool>> DeleteProject(Guid id);
		Task<List<ProjectTaskDto>> GetProjectTasks(Guid projectId, ProjectTaskQueryParameters requestDto);

	}
}
