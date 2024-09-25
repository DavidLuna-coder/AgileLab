using Front.ApiClient.Interfaces;
using Shared.DTOs;

namespace Front.ApiClient.Implementations
{
    public class ProjectsApi(IApiHttpClient client) : IProjectsApi
    {
        public Task CreateProject()
        {
            return Task.CompletedTask;
        }

        public Task GetProjects()
        {
            return Task.CompletedTask;
        }
    }
}
