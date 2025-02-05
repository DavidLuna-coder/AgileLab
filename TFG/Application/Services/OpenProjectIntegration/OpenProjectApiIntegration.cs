using Shared.DTOs;
using System.Text.Json;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Services.OpenProjectIntegration.Dtos;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Services.OpenProjectIntegration
{
	public class OpenProjectApiIntegration(OpenProjectApi api) : IOpenProjectApiIntegration
	{
		private readonly OpenProjectApi _api = api;
		private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
		public record OpenProjectUserRegistrationResponse(int Id);
		public record OpenProjectProjectCreatedResponse(int Id);
		public async Task<Result<int>> CreateProject(OpenProjectCreateProjectDto project)
		{
			try
			{
				var response = await _api.PostAsync("projects", project);
				string responseBody = await response.Content.ReadAsStringAsync();
				OpenProjectProjectCreatedResponse createdProject = JsonSerializer.Deserialize<OpenProjectProjectCreatedResponse>(responseBody, _serializerOptions) ?? throw new Exception("Error creating project");
				return createdProject.Id;
			}
			catch (Exception ex)
			{
				return new Result<int>([$"OpenProject Project Creation Error: {ex.Message}"]);
			}
		}
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

		public async Task<Result<bool>> DeleteProject(int projectId)
		{
			try
			{
				var response = await _api.DeleteAsync($"projects/{projectId}");
				return true;
			}
			catch (Exception ex)
			{
				return new Result<bool>([ex.Message]);
			}
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

		public async Task<Result<bool>> CreateMembership(int userId, int projectId, int[] rolesId)
		{
			OpenProjectCreateMembershipsDtos creationRequest = new()
			{
				_links = new()
				{
					Principal = new() { Href = $"/api/v3/users/{userId}" },
					Project = new() { Href = $"/api/v3/projects/{projectId}" },
					Roles = rolesId.Select(roleId => new OpenProjectLink() { Href = $"/api/v3/roles/{roleId}" }).ToArray()
				}
			};
			try
			{
				var response = await _api.PostAsync("memberships", creationRequest);
				return true;
			}
			catch (Exception ex)
			{
				return new Result<bool>([ex.Message]);
			}

		}
	}
}