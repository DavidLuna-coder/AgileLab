using Shared.DTOs.Filters;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using Shared.DTOs.Projects.Metrics;
using Shared.DTOs.Users;

namespace Front.ApiClient.Interfaces
{
    public interface IProjectsApi
    {
        Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto);
        Task<ProjectDto> UpdateProject(Guid ProjectId, UpdateProjectDto updatedProject);
        Task<PaginatedResponseDto<FilteredProjectDto>> GetProjects(FilteredPaginatedRequestDto<ProjectQueryParameters> request);
        Task<ProjectDto> GetProject(Guid projectId);
        Task<PaginatedResponseDto<FilteredUserDto>> GetProjectUsers(Guid projectId, FilteredPaginatedRequestDto<GetUsersQueryParameters> request);
        Task DeleteProject(Guid projectId);
        Task<PaginatedResponseDto<TaskSummaryDto>> GetTaskSummary(Guid projectid, FilteredPaginatedRequestDto<GetTaskSummaryQueryFilters> request);
        Task<ProjectMetricsDto> GetProjectMetrics(Guid projectId);
        Task<List<AffectedFileDto>> GetMostAffectedFiles(Guid projectId);
        Task<GitlabMetricsDto> GetGitlabMetrics(Guid projectId, GetGitlabMetricsDto getGitlabMetricsDto);
        Task<OpenProjectMetricsDto> GetOpenProjectMetrics(Guid projectId, GetOpenProjectMetricsDto getOpenProjectMetricsDto);
	}
}
