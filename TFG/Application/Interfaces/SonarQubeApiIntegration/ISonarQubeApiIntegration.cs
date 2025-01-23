using Shared.DTOs;
using TFG.Application.Services.Dtos;
using TFG.Domain.Results;

namespace TFG.Application.Interfaces.SonarQubeIntegration
{
	public interface ISonarQubeApiIntegration
    {
        public Task<Result<string>> CreateUser(RegistrationDto registrationDto);
        public Task<Result<bool>> DeleteUser(string id);
        public Task<Result<SonarQubeCreateProjectResponseDto>> CreateProject(SonarQubeCreateProjectRequestDto request);
        public Task<Result<SonarQubeGetDopSettingsDto>> GetDopSettings();
        public Task<Result<bool>> DeleteProject(SonarQubeDeleteProjectDto projectToDelete);
	}
}
