using System.IdentityModel.Tokens.Jwt;
using TFG.Application.Dtos;
using TFG.Application.Interfaces;
using TFG.Domain;
using TFG.Domain.Results;

namespace TFG.Infrastructure.Security
{
	public class TokenService : ITokenService
	{
		public Result<UserInfo> ExtractUserInfo(string token)
		{
			var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

			try
			{
				UserInfo userInfo = new()
				{
					Email = jwtToken.Claims.First(c => c.Type == ClaimConstants.Email).Value,
					Username = jwtToken.Claims.First(c => c.Type == ClaimConstants.Username).Value,
					UserId = jwtToken.Claims.First(c => c.Type == ClaimConstants.UserId).Value,
					IsAdmin = bool.Parse(jwtToken.Claims.First(c => c.Type == ClaimConstants.IsAdmin).Value)
				};

				return userInfo;
			}
			catch
			{
				return new Result<UserInfo>(["Invalid token"]);
			}
		}
	}
}
