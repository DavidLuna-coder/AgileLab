using Microsoft.AspNetCore.Mvc;
using Shared.DTOs;
using TFG.Application.Interfaces;

namespace TFG.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto registrationDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registrationDto);
                if(result.Succeeded) return Ok(result);
                return BadRequest(result.Errors);
                
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            var result = await _authService.LoginAsync(model);
            if (!result.Success) return Unauthorized(result.Errors);

            return Ok(result.Value);
        }
    }
}
