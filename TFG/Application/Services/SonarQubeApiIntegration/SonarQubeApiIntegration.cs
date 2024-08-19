using Shared.DTOs;
using System.Text.Json;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Domain.Results;

namespace TFG.Application.Services.SonarQubeIntegration
{
    public class SonarQubeApiIntegration(SonarQubeApi sonarQubeApi) : ISonarQubeApiIntegration
    {
        private readonly SonarQubeApi _sonarQubeApi = sonarQubeApi;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

        public record SonarQubeUserCreatedResponse(string Id);
        public async Task<Result<string>> CreateUser(RegistrationDto registrationDto)
        {
            SonarQubeCreateUserRequestDto request = new()
            {
                Email = registrationDto.Email,
                Password = registrationDto.Password,
                Local = true,
                Name = registrationDto.UserName,
                ScmAccounts = [],
            };
            try
            {
                var response = await _sonarQubeApi.PostAsync("users-management/users", request, "v2");
                string responseBody = await response.Content.ReadAsStringAsync();

                SonarQubeUserCreatedResponse responseObject = JsonSerializer.Deserialize<SonarQubeUserCreatedResponse>(responseBody, _serializerOptions) ?? throw new Exception("Failed user deserialization");

                return responseObject.Id;
            }
            catch (Exception ex)
            {
                return new Result<string>([ex.Message]);
            }
        }

        public async Task<Result<bool>> DeleteUser(string id)
        {
            await _sonarQubeApi.DeleteAsync($"users-management/users/{id}", "v2");
            throw new NotImplementedException();
        }
    }
}
