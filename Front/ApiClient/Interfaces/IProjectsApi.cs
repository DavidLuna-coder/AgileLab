using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using Shared.DTOs.Users;

namespace Front.ApiClient.Interfaces
{
    public interface IProjectsApi
    {
        public Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto);
        public Task<ProjectDto> UpdateProject(Guid ProjectId, UpdateProjectDto updatedProject);
        public Task<PaginatedResponseDto<FilteredProjectDto>> GetProjects(FilteredPaginatedRequestDto<ProjectQueryParameters> request);
        public Task<ProjectDto> GetProject(Guid projectId);
        public Task<PaginatedResponseDto<FilteredUserDto>> GetProjectUsers(Guid projectId, FilteredPaginatedRequestDto<GetUsersQueryParameters> request);
        public Task DeleteProject(Guid projectId);
	}
}
