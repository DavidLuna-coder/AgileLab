using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TFG.Application.Interfaces;
using TFG.Application.Interfaces.GitlabApiIntegration;
using TFG.Application.Interfaces.OpenProjectApiIntegration;
using TFG.Domain.Results;
using TFG.Infrastructure.Data;
using TFG.Model.Entities;

namespace TFG.Application.Services.Auth
{
    public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration, IGitlabApiIntegration gitlabApiIntegration, IOpenProjectApiIntegration openProjectApiIntegration) : IAuthService
    {
        private readonly UserManager<User> _userManager = userManager;
        private readonly SignInManager<User> _signInManager = signInManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IGitlabApiIntegration _gitlabApiIntegration = gitlabApiIntegration;
        private readonly IOpenProjectApiIntegration _openProjectApiIntegration = openProjectApiIntegration;
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
            };

            bool userCreated = false;
            try
            {

                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded) return result;
                userCreated = true;
                var gitlabResult = await RegisterGitlab(model);
                if (!gitlabResult.Succeeded)
                {

                    await _userManager.DeleteAsync(user);
                    return gitlabResult;
                }

                var openProjectResult = await RegisterOpenProject(model);
                if (!openProjectResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return openProjectResult;
                }

                return IdentityResult.Success;

            }
            catch (Exception ex)
            {
                if (userCreated)
                {
                    await _userManager.DeleteAsync(user);
                }
                var errors = new[]
                {
                    new IdentityError { Description = ex.Message },
                };
                return IdentityResult.Failed(errors);
            }
        }

        private async Task<IdentityResult> RegisterOpenProject(RegistrationDto model)
        {
            var openProjectResult = await _openProjectApiIntegration.CreateUser(model);
            if (!openProjectResult.Success)
            {
                var errors = new[]
                {
                   new IdentityError { Description = string.Join(",", openProjectResult.Errors) },
                };

                return IdentityResult.Failed(errors);
            }
            return IdentityResult.Success;
        }

        private async Task<IdentityResult> RegisterGitlab(RegistrationDto model)
        {
            var gitLabResult = await _gitlabApiIntegration.CreateUser(model);
            if (!gitLabResult.Success)
            {

                var errors = new[]
                {
                        new IdentityError { Description = string.Join(",", gitLabResult.Errors) },
                    };

                return IdentityResult.Failed(errors);
            }

            return IdentityResult.Success;
        }

        public async Task<Result<string>> LoginAsync(RegistrationDto model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
            if (!result.Succeeded)
            {
                return new Result<string>(["Login failed"]);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = GenerateJwtToken(user);
            return token;
        }
        private string GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings").Get<JwtSettings>();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var token = new JwtSecurityToken(
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
