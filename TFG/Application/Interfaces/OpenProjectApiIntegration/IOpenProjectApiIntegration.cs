using Shared.DTOs;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.OpenProjectApiIntegration
{
    public interface IOpenProjectApiIntegration
    {
        Task<Result<int>> CreateUser(RegistrationDto user);
        Task CreateProject(Project project);
        Task<Result<bool>> DeleteUser(User user);
        Task DeleteProject(Project project);
    }
}
