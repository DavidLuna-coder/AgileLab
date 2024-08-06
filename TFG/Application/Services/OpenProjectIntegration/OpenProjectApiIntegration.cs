using Shared.DTOs;
using System.Text.Json;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Services.OpenProjectIntegration
{
    public class OpenProjectApiIntegration(OpenProjectApi api) : IOpenProjectApiIntegration
    {
        private readonly OpenProjectApi _api = api;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

        public Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }
        public record OpenProjectUserRegistrationResponse(int Id);
        public async Task<Result<int>> CreateUser(RegistrationDto user)
        {
            OpenProjectCreateUserRequest request = new()
            {
                Admin = user.IsAdmin,
                Email = user.Email,
                Login = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Language = "es",
                Password = user.Password
            };
            try
            {
                var response = await _api.PostAsync("users", request);
                string responseBody = await response.Content.ReadAsStringAsync();

                OpenProjectUserRegistrationResponse responseObject = JsonSerializer.Deserialize<OpenProjectUserRegistrationResponse>(responseBody, _serializerOptions) ?? throw new Exception("Failed user deserialization");
                return responseObject.Id;
            }
            catch (Exception ex)
            {
                return new Result<int>([ex.Message]);
            }
        }

        public Task DeleteProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> DeleteUser(User user)
        {
            try
            {
                var response = await _api.DeleteAsync($"users/{user.OpenProjectId}");
                return true;
            }
            catch (Exception ex)
            {
                return new Result<bool>([ex.Message]);
            }
        }
    }
}
