using Shared.DTOs;
using Shared.DTOs.Projects;
using System.Text.Json;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Services.GitlabIntegration.Dtos;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Services.GitlabIntegration
{
	public class GitlabApiIntegration(GitLabApi api) : IGitlabApiIntegration
    {
        private readonly GitLabApi _api = api;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
        public record GitlabUserRegistrationResponse(int Id);

		public async Task<Result<bool>> CreateProject(CreateProjectDto project, int gitlabUserId)
        {
			GitlabCreateProjectDto gitlabProject = new()
			{
				Name = project.Name,
                UserId = gitlabUserId,
			};
            try
            {
                var response = await _api.PostAsync($"projects/user/{gitlabUserId}", gitlabProject);
                return true;
			}
            catch (Exception ex)
            {
                return new Result<bool>([ex.Message]);
			}
		}

        public async Task<Result<int>> CreateUser(RegistrationDto user)
        {
            GitlabUserRequestDto gitlabUserRequest = new()
            {
                Email = user.Email,
                Name = user.Email,
                Password = user.Password,
                Username = user.UserName
            };
            try
            {
                var response = await _api.PostAsync("users", gitlabUserRequest);
                string responseBody = await response.Content.ReadAsStringAsync();

                GitlabUserRegistrationResponse responseObject = JsonSerializer.Deserialize<GitlabUserRegistrationResponse>(responseBody, _serializerOptions) ?? throw new Exception("Failed user deserialization");

                return responseObject.Id;
            } catch (Exception ex)
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
                await _api.DeleteAsync($"users/{user.Id}");
                return true;
            }
            catch (Exception ex)
            {
                return new Result<bool>([ex.Message]);
            }
        }
    }
}
