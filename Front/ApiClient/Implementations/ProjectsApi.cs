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

		public Task<GitlabMetricsDto> GetGitlabMetrics(Guid projectId, GetGitlabMetricsDto getGitlabMetricsDto)
		{
			return client.PostAsync<GetGitlabMetricsDto, GitlabMetricsDto>($"{PROJECTS_ENDPOINT}/{projectId}/gitlab-metrics", getGitlabMetricsDto);
		}

		public Task<List<AffectedFileDto>> GetMostAffectedFiles(Guid projectId)
		{
			return client.GetAsync<List<AffectedFileDto>>($"{PROJECTS_ENDPOINT}/{projectId}/metrics/most-affected-files");
		}

		public Task<OpenProjectMetricsDto> GetOpenProjectMetrics(Guid projectId, GetOpenProjectMetricsDto getOpenProjectMetricsDto)
		{
			return client.PostAsync<GetOpenProjectMetricsDto, OpenProjectMetricsDto>($"{PROJECTS_ENDPOINT}/{projectId}/openproject-metrics", getOpenProjectMetricsDto);
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

		public Task ArchiveProject(Guid projectId)
		{
			return client.PostAsync<object>(
				$"{PROJECTS_ENDPOINT}/{projectId}/archive", null);
		}

		public Task UnarchiveProject(Guid projectId)
		{
			return client.PostAsync<object>(
				$"{PROJECTS_ENDPOINT}/{projectId}/unarchive", null);
		}
	}
}
