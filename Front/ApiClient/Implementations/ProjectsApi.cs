using Front.ApiClient.Interfaces;
using Shared.DTOs.Projects;

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

		public async Task<IEnumerable<PaginatedProjectDto>> GetProjects()
        {
			IEnumerable<PaginatedProjectDto> projects = await client.GetAsync<IEnumerable<PaginatedProjectDto>>(PROJECTS_ENDPOINT);
			return projects;
		}
    }
}
