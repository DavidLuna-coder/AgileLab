using Microsoft.AspNetCore.Identity;
using Shared.DTOs;
using TFG.Domain.Results;
using TFG.Model.Entities;

namespace TFG.Application.Interfaces
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegistrationDto model);
        Task<Result<string>> LoginAsync(RegistrationDto model);
    }
}
