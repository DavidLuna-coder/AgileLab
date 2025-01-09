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

		public async Task<Result<GitlabCreateProjectResponseDto>> CreateProject(CreateProjectDto project, int gitlabUserId)
		{
			GitlabCreateProjectDto gitlabProject = new()
			{
				Name = project.Name,
				UserId = gitlabUserId,
			};
			try
			{
				var response = await _api.PostAsync($"projects/user/{gitlabUserId}", gitlabProject);
				string responseBody = await response.Content.ReadAsStringAsync();
				GitlabCreateProjectResponseDto createdProject = JsonSerializer.Deserialize<GitlabCreateProjectResponseDto>(responseBody, _serializerOptions) ?? throw new Exception("Error creating project");
				return createdProject;
			}
			catch (Exception ex)
			{
				return new Result<GitlabCreateProjectResponseDto>([ex.Message]);
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
			}
			catch (Exception ex)
			{
				return new Result<int>([ex.Message]);
			}
		}

		public async Task<Result<bool>> DeleteProject(string gitlabProjectId)
		{
			try
			{
				var response = await _api.DeleteAsync($"projects/{gitlabProjectId}");
				return true;
			}
			catch
			{
				return new Result<bool>(["An error ocurred deleting the project in gitlab"]);
			}
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

		public async Task<Result<bool>> AddUsersToProject(GitlabAddMembersToProjectDto dto)
		{
			try
			{
				var response = await _api.PostAsync($"projects/{dto.Id}/members", dto);
				return true;
			}
			catch (Exception ex)
			{
				return new Result<bool>([$"Gitlab AddUsersToProjectError: {ex.Message}"]);
			}
		}

		public async Task<Result<GitlabProjectMembersResponseDto>> GetProjectMembers(int projectId)
		{
			try
			{
				var response = await _api.GetAsync($"projects/{projectId}/members");
				string responseBody = await response.Content.ReadAsStringAsync();
				GitlabProjectMembersResponseDto members = JsonSerializer.Deserialize<GitlabProjectMembersResponseDto>(responseBody, _serializerOptions) ?? throw new Exception("Error getting project members");
				return members;
			}
			catch (Exception ex)
			{
				return new Result<GitlabProjectMembersResponseDto>([$"Gitlab GetProjectMembersError: {ex.Message}"]);
			}
		}
	}
}
