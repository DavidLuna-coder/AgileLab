using Shared.DTOs;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces.SonarQubeIntegration
{
    public interface ISonarQubeApiIntegration
    {
        public Task<Result<string>> CreateUser(RegistrationDto registrationDto);
        public Task<Result<bool>> DeleteUser(string id);
    }
}
