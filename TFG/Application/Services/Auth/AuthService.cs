using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NGitLab;
using NGitLab.Models;
using Shared.DTOs;
using Shared.Utils.DateTimeProvider;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TFG.Application.Interfaces;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Application.Interfaces.SonarQubeIntegration;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using User = TFG.Model.Entities.User;

namespace TFG.Application.Services.Auth
{
	public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IOpenProjectApiIntegration openProjectApiIntegration, ISonarQubeApiIntegration sonarQubeApiIntegration, IDateTimeProvider dateTimeProvider, IGitLabClient gitLabClient) : IAuthService
	{
		private readonly UserManager<User> _userManager = userManager;
		private readonly SignInManager<User> _signInManager = signInManager;
		private readonly IConfiguration _configuration = configuration;
		private readonly IOpenProjectApiIntegration _openProjectApiIntegration = openProjectApiIntegration;
		private readonly ISonarQubeApiIntegration _sonarQubeApiIntegration = sonarQubeApiIntegration;
		private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
		private readonly IGitLabClient gitLabClient = gitLabClient;
		#region REGISTER
		public async Task<IdentityResult> RegisterAsync(RegistrationDto model)
		{

			var user = new User()
			{
				Email = model.Email,
				EmailConfirmed = true,
				UserName = model.UserName,
				IsAdmin = model.IsAdmin,
				FirstName = model.FirstName,
				LastName = model.LastName,
				OpenProjectId = string.Empty,
				GitlabId = string.Empty,
				SonarQubeId = string.Empty,
			};

			bool userCreated = false;
			try
			{
				//Gitlab registration
				Result<long> gitlabResult = await RegisterGitlab(model);
				if (!gitlabResult.Success) return CreateIdentityError(gitlabResult.Errors);
				user.GitlabId = gitlabResult.Value.ToString();

				//OpenProject registration
				var openProjectResult = await RegisterOpenProject(model);
				if (!openProjectResult.Success)
				{
					gitLabClient.Users.Delete(long.Parse(user.GitlabId));
					return CreateIdentityError(openProjectResult.Errors);
				}
				user.OpenProjectId = openProjectResult.Value.ToString();

				//SonarQube registration Falta por implementar.
				var sonarQubeResult = await _sonarQubeApiIntegration.CreateUser(model);
				if (!sonarQubeResult.Success)
				{
					gitLabClient.Users.Delete(long.Parse(user.GitlabId));
					await _openProjectApiIntegration.DeleteUser(user);
					return CreateIdentityError(sonarQubeResult.Errors);
				}
				user.SonarQubeId = sonarQubeResult.Value;

				//App registration
				                                                                                                                                                                
				var result = await _userManager.CreateAsync(user, model.Password);
				if (!result.Succeeded)
				{
					gitLabClient.Users.Delete(long.Parse(user.GitlabId));
					await _openProjectApiIntegration.DeleteUser(user);
					await _sonarQubeApiIntegration.DeleteUser(user.SonarQubeId);
				}

				return result;

			}
			catch (Exception ex)
			{
				if (userCreated)
				{
					await _userManager.DeleteAsync(user);
					gitLabClient.Users.Delete(long.Parse(user.GitlabId));
				}
				var errors = new[]
				{
					new IdentityError { Description = ex.Message },
				};
				return IdentityResult.Failed(errors);
			}
		}

		private async Task<Result<int>> RegisterOpenProject(RegistrationDto model)
		{
			var openProjectResult = await _openProjectApiIntegration.CreateUser(model);
			return openProjectResult;
		}

		private async Task<Result<long>> RegisterGitlab(RegistrationDto model)
		{
			UserUpsert user = new()
			{
				Email = model.Email,
				Password = model.Password,
				Name = model.Email,
				Username = model.UserName
			};
			try
			{
				NGitLab.Models.User gitlabUser = await gitLabClient.Users.CreateAsync(user);
				return gitlabUser.Id;
			}
			catch(Exception ex)
			{
				return new([ex.Message]);
			}
		}
		#endregion

		#region LOGIN
		public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto model)
		{
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null) return new Result<LoginResponseDto>(["User Not found"]);
			var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
			if (!result.Succeeded)
			{
				return new Result<LoginResponseDto>(["Login failed"]);
			}

			var token = GenerateJwtToken(user);
			return new LoginResponseDto()
			{
				ExpirationDate = DateTime.UtcNow.AddMinutes(1440),
				Token = token,
			};
		}

		private string GenerateJwtToken(User user)
		{
			var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
			var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
				new Claim("IsAdmin", user.IsAdmin.ToString()),
				new Claim("Id", user.Id),
			};

			var token = new JwtSecurityToken(
				issuer: jwtSettings.Issuer,
				audience: jwtSettings.Audience,
				claims: claims,
				expires: _dateTimeProvider.UtcNow.AddMinutes(1440),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		#endregion


		private IdentityResult CreateIdentityError(ImmutableArray<string> error)
		{
			var errors = new[]
			{
				new IdentityError { Description = string.Join(",", error) },
			};
			return IdentityResult.Failed(errors);
		}
	}
}
