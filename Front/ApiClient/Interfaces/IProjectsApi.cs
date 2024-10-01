using Shared.DTOs.Projects;

namespace Front.ApiClient.Interfaces
{
    public interface IProjectsApi
    {
        public Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto);
        public Task<IEnumerable<PaginatedProjectDto>> GetProjects();

    }
}
