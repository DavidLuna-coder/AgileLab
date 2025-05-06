using Front.ApiClient.Interfaces;
using Shared.DTOs.Filters;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using Shared.DTOs.Projects.Metrics;
using Shared.DTOs.Users;

namespace Front.ApiClient.Implementations
{
	public class ProjectsApi(IApiHttpClient client) : IProjectsApi
	{
		private const string PROJECTS_ENDPOINT = "api/projects";
		public Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto)
		{
			return client.PostAsync<CreateProjectDto, ProjectDto>(PROJECTS_ENDPOINT, createProjectDto);
		}

		public async Task DeleteProject(Guid projectId)
		{
			await client.DeleteAsync($"{PROJECTS_ENDPOINT}/{projectId}");
		}

		public Task<ProjectDto> GetProject(Guid projectId)
		{
			return client.GetAsync<ProjectDto>($"{PROJECTS_ENDPOINT}/{projectId}");
		}

		public Task<ProjectMetricsDto> GetProjectMetrics(Guid projectId)
		{
			return client.GetAsync<ProjectMetricsDto>($"{PROJECTS_ENDPOINT}/{projectId}/metrics");
		}

		public Task<PaginatedResponseDto<FilteredProjectDto>> GetProjects(FilteredPaginatedRequestDto<ProjectQueryParameters> request)
		{
			return client.PostAsync<FilteredPaginatedRequestDto<ProjectQueryParameters>, PaginatedResponseDto<FilteredProjectDto>>($"{PROJECTS_ENDPOINT}/search", request);			
		}

		public Task<PaginatedResponseDto<FilteredUserDto>> GetProjectUsers(Guid projectId, FilteredPaginatedRequestDto<GetUsersQueryParameters> request)
		{
			return client.PostAsync<FilteredPaginatedRequestDto<GetUsersQueryParameters>, PaginatedResponseDto<FilteredUserDto>>($"{PROJECTS_ENDPOINT}/{projectId}/users/search", request);
		}

		public Task<PaginatedResponseDto<TaskSummaryDto>> GetTaskSummary(Guid projectid, FilteredPaginatedRequestDto<GetTaskSummaryQueryFilters> request)
		{
			return client.PostAsync<FilteredPaginatedRequestDto<GetTaskSummaryQueryFilters>, PaginatedResponseDto<TaskSummaryDto>>($"{PROJECTS_ENDPOINT}/{projectid}/task-summary/search", request);
		}

		public Task<ProjectDto> UpdateProject(Guid projectId,UpdateProjectDto updatedProject)
		{
			return client.PutAsync<UpdateProjectDto, ProjectDto>($"{PROJECTS_ENDPOINT}/{projectId}", updatedProject);
		}
	}
}
