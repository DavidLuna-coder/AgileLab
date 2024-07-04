using Shared.DTOs;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Services.GitlabIntegration
{
    public class GitlabApiIntegration(GitLabApi api) : IGitlabApiIntegration
    {
        private readonly GitLabApi _api = api;
        private JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
        public Task CreateProject(Project project)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> CreateUser(RegistrationDto user)
        {
            GitlabUserRequest gitlabUserRequest = new()
            {
                Email = user.Email,
                Name = user.Email,
                Password = user.Password,
                Username = user.UserName
            };
            string json = JsonSerializer.Serialize(gitlabUserRequest, _serializerOptions);
            try
            {
                var response = await _api.PostAsync("users", gitlabUserRequest);
                return true;
            } catch (Exception ex)
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
