using Shared.DTOs;
using System.Text.Json;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Application.Services.Dtos;
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
				Login = registrationDto.UserName,
				Local = true,
				Name = $"{registrationDto.FirstName} {registrationDto.LastName}",
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

		public async Task<Result<SonarQubeCreateProjectResponseDto>> CreateProject(SonarQubeCreateProjectRequestDto request)
		{
			try
			{
				var createProjectResponseMessage = await _sonarQubeApi.PostAsync("dop-translation/bound-projects", request, "v2");
				string responseBody = await createProjectResponseMessage.Content.ReadAsStringAsync();
				SonarQubeCreateProjectResponseDto responseObject = JsonSerializer.Deserialize<SonarQubeCreateProjectResponseDto>(responseBody, _serializerOptions) ?? throw new Exception("Failed project deserialization");

				return responseObject;
			}
			catch (Exception ex)
			{
				return new Result<SonarQubeCreateProjectResponseDto>([$"SonarQubeCreateProjectError:{ex.Message}"]);
			}
		}

		public async Task<Result<SonarQubeGetDopSettingsDto>> GetDopSettings()
		{
			try
			{
				var responseMessage = await _sonarQubeApi.GetAsync("dop-translation/dop-settings", "v2");
				string responseBody = await responseMessage.Content.ReadAsStringAsync();
				SonarQubeGetDopSettingsDto responseObject = JsonSerializer.Deserialize<SonarQubeGetDopSettingsDto>(responseBody, _serializerOptions) ?? throw new Exception("Failed dop settings deserialization");
				return responseObject;
			}
			catch (Exception ex)
			{
				return new Result<SonarQubeGetDopSettingsDto>([$"SonarQubeGetDopSettingsError:{ex.Message}"]);
			}
		}

		public async Task<Result<bool>> DeleteProject(SonarQubeDeleteProjectDto projectToDelete)
		{
			try
			{
				await _sonarQubeApi.PostAsync($"projects/delete?project={projectToDelete.Project}", projectToDelete);
				return true;
			}
			catch(Exception ex)
			{
				return new Result<bool>([$"SonarQubeDeleteError:{ex.Message}"]);
			}
		}
	}
}
