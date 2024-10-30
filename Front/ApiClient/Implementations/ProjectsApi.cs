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

		public async Task<ProjectDto> GetProject(Guid projectId)
		{
			var response = await client.GetAsync<ProjectDto>($"{PROJECTS_ENDPOINT}/{projectId}");
			return response;
		}

		public async Task<PaginatedResponseDto<FilteredProjectDto>> GetProjects(PaginatedRequestDto<ProjectQueryParameters> request)
		{
			var response = await client.PostAsync<PaginatedRequestDto<ProjectQueryParameters>, PaginatedResponseDto<FilteredProjectDto>>($"{PROJECTS_ENDPOINT}/search", request);			
			return response;
		}

		public async Task<PaginatedResponseDto<FilteredUserDto>> GetProjectUsers(Guid projectId, PaginatedRequestDto<GetUsersQueryParameters> request)
		{
			var response = await client.PostAsync<PaginatedRequestDto<GetUsersQueryParameters>, PaginatedResponseDto<FilteredUserDto>>($"{PROJECTS_ENDPOINT}/{projectId}/users/search", request);
			return response;
		}
	}
}
