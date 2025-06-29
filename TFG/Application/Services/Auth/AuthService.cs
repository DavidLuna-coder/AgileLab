using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using TFG.OpenProjectClient;
using TFG.OpenProjectClient.Models.Users;
using TFG.SonarQubeClient;
using User = TFG.Domain.Entities.User;

namespace TFG.Application.Services.Auth
{
	public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, ISonarQubeClient sonarQubeClient, IDateTimeProvider dateTimeProvider, IGitLabClient gitLabClient, IOpenProjectClient openProjectClient, ILogger<AuthService> logger) : IAuthService
	{
		private readonly UserManager<User> _userManager = userManager;
		private readonly SignInManager<User> _signInManager = signInManager;
		private readonly IConfiguration _configuration = configuration;
		private readonly ISonarQubeClient _sonarQubeClient = sonarQubeClient;
		private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
		private readonly IGitLabClient _gitLabClient = gitLabClient;
		private readonly IOpenProjectClient _openProjectClient = openProjectClient;
		private readonly ILogger<AuthService> _logger = logger;

		#region REGISTER
		public async Task<IdentityResult> RegisterAsync(RegistrationDto model)
		{

			var user = new User()
			{
				Email = model.Email,
				EmailConfirmed = true,
				UserName = model.UserName,
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
				UserCreated openProjectUser = await RegisterOpenProject(model);
				user.OpenProjectId = openProjectUser.Id.ToString();

				//SonarQube registration Falta por implementar.
				SonarQubeClient.Models.UserCreation userCreation = new()
				{
					Login = model.UserName,
					Name = model.FirstName + " " + model.LastName,
					Email = model.Email,
					Password = model.Password,
				};
				var sonarUser = await _sonarQubeClient.Users.CreateAsync(userCreation);

				user.SonarQubeId = sonarUser.Id;

				//App registration

				var result = await _userManager.CreateAsync(user, model.Password);
				if (!result.Succeeded)
				{
					_gitLabClient.Users.Delete(long.Parse(user.GitlabId));
					await _openProjectClient.Users.DeleteAsync(int.Parse(user.OpenProjectId));
					await _sonarQubeClient.Users.DeleteAsync(user.SonarQubeId);
				}

				return result;

			}
			catch (Exception)
			{
				if (userCreated)
				{
					await _userManager.DeleteAsync(user);
					_gitLabClient.Users.Delete(long.Parse(user.GitlabId));
				}
				throw;
			}
		}

		private async Task<UserCreated> RegisterOpenProject(RegistrationDto model)
		{
			OpenProjectClient.Models.Users.UserCreation userToCreate = new()
			{
				Email = model.Email,
				Login = model.UserName,
				FirstName = model.FirstName,
				LastName = model.LastName,
				Language = "es",
				Password = model.Password
			};
			UserCreated userCreated = await _openProjectClient.Users.CreateAsync(userToCreate);
			return userCreated;
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
				NGitLab.Models.User gitlabUser = await _gitLabClient.Users.CreateAsync(user);
				return gitlabUser.Id;
			}
			catch (Exception ex)
			{
				return new([ex.Message]);
			}
		}
		#endregion

		#region LOGIN
		public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto model)
		{
			_logger.LogInformation("Intentando login para email: {Email}", model.Email);
			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
			{
				_logger.LogWarning("Usuario no encontrado para email: {Email}", model.Email);
				return new Result<LoginResponseDto>(["User Not found"]);
			}
			var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
			if (!result.Succeeded)
			{
				_logger.LogWarning("Login fallido para email: {Email} (UserName: {UserName})", model.Email, user.UserName);
				return new Result<LoginResponseDto>(["Login failed"]);
			}
			_logger.LogInformation("Login exitoso para email: {Email} (UserName: {UserName})", model.Email, user.UserName);

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
