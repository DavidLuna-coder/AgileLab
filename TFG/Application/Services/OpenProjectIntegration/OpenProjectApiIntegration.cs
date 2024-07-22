using Shared.DTOs;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Services.OpenProjectIntegration
{
    public class OpenProjectApiIntegration(OpenProjectApi api) : IOpenProjectApiIntegration
    {
        private readonly OpenProjectApi _api = api;
        public Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> CreateUser(RegistrationDto user)
        {
            OpenProjectCreateUserRequest request = new()
            {
                Admin = user.IsAdmin,
                Email = user.Email,
                Login = user.UserName,
                FirstName = user.UserName,
                LastName = user.UserName,
                Language = "es",
                Password = user.Password
            };
            try
            {
                var response = await _api.PostAsync("users", request);
                return true;
            }
            catch (Exception ex)
            {
                return new Result<bool>([ex.Message]);
            }
        }

        public Task DeleteProject(Project project)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
