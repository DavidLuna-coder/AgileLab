using Shared.DTOs;
using TFG.Application.Services.OpenProjectIntegration;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.OpenProjectApiIntegration
{
    public interface IOpenProjectApiIntegration
    {
        Task<Result<int>> CreateUser(RegistrationDto user);
        Task<Result<int>> CreateProject(OpenProjectCreateProjectDto project);
        Task<Result<bool>> DeleteUser(User user);
        Task<Result<bool>> DeleteProject(int projectId);
        Task<Result<bool>> CreateMembership(int userId, int projectId, int[] rolesId);
        Task<Result<OpenProjectWorkPackage[]>> GetWorkPackages(int projectId, OpenProjectFilterBuilder filterBuilder);
	}
}
