using Shared.DTOs;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.GitlabApiIntegration
{
    public interface IGitlabApiIntegration
    {
        Task<Result<bool>> CreateUser(RegistrationDto user);
        Task CreateProject(Project project);
        Task DeleteUser(User user);
        Task DeleteProject(Project project);
    }
}
