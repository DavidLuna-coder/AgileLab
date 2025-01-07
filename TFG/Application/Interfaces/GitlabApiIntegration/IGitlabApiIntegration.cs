using Shared.DTOs;
using Shared.DTOs.Projects;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.GitlabApiIntegration
{
    public interface IGitlabApiIntegration
    {
        Task<Result<int>> CreateUser(RegistrationDto user);
		//<summary>
		//Creates a project in Gitlab
		//<param name="project">The project to be created</param>
		//<param name="gitlabUserId">The id of the user that will own the project</param>
		//</summary>
		Task<Result<bool>> CreateProject(CreateProjectDto project, int gitlabUserId);
        Task<Result<bool>> DeleteUser(User user);
        Task DeleteProject(Project project);
    }
}
