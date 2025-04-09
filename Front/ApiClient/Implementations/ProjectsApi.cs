using Front.ApiClient.Interfaces;
using Shared.DTOs.Pagination;
using Shared.DTOs.Projects;
using Shared.DTOs.Users;

namespace Front.ApiClient.Implementations
{
	public class ProjectsApi(IApiHttpClient client) : IProjectsApi
	{
		private const string PROJECTS_ENDPOINT = "api/projects";
		public async Task<ProjectDto> CreateProject(CreateProjectDto createProjectDto)
		{
			ProjectDto createdProject = await client.PostAsync<CreateProjectDto, ProjectDto>(PROJECTS_ENDPOINT, createProjectDto);
			return createdProject;
		}

		public async Task DeleteProject(Guid projectId)
		{
			await client.DeleteAsync($"{PROJECTS_ENDPOINT}/{projectId}");
		}

		public async Task<ProjectDto> GetProject(Guid projectId)
		{
			var response = await client.GetAsync<ProjectDto>($"{PROJECTS_ENDPOINT}/{projectId}");
			return response;
		}

		public async Task<PaginatedResponseDto<FilteredProjectDto>> GetProjects(FilteredPaginatedRequestDto<ProjectQueryParameters> request)
		{
			var response = await client.PostAsync<FilteredPaginatedRequestDto<ProjectQueryParameters>, PaginatedResponseDto<FilteredProjectDto>>($"{PROJECTS_ENDPOINT}/search", request);			
			return response;
		}

		public async Task<PaginatedResponseDto<FilteredUserDto>> GetProjectUsers(Guid projectId, FilteredPaginatedRequestDto<GetUsersQueryParameters> request)
		{
			var response = await client.PostAsync<FilteredPaginatedRequestDto<GetUsersQueryParameters>, PaginatedResponseDto<FilteredUserDto>>($"{PROJECTS_ENDPOINT}/{projectId}/users/search", request);
			return response;
		}

		public async Task<ProjectDto> UpdateProject(Guid projectId,UpdateProjectDto updatedProject)
		{
			ProjectDto result = await client.PutAsync<UpdateProjectDto, ProjectDto>($"{PROJECTS_ENDPOINT}/{projectId}", updatedProject);
			return result;
		}
	}
}
