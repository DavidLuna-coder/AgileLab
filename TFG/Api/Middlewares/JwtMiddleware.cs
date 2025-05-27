using TFG.Application.Dtos;
using TFG.Application.Interfaces;
using TFG.Application.Security;
using TFG.Domain.Results;
using TFG.Infrastructure.Security;

namespace TFG.Api.Middlewares
{
	public class JwtMiddleware
	{
		private readonly RequestDelegate _next;
		public JwtMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

			if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
			{
				var token = authorizationHeader.Substring("Bearer ".Length).Trim();
				ITokenService tokenService = new TokenService();
				Result<UserInfo> result = tokenService.ExtractUserInfo(token);
				if (!result.Success)
				{
					context.Response.StatusCode = 401;
					await context.Response.WriteAsync("Invalid token");
					return;
				}

				context.Items[nameof(IUserInfo)] = result.Value;
			}
			await _next(context);
		}
	}
}
