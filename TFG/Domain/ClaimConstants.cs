using System.IdentityModel.Tokens.Jwt;

namespace TFG.Domain
{
	public static class ClaimConstants
	{
		public const string Email = JwtRegisteredClaimNames.Email;
		public const string Username = JwtRegisteredClaimNames.UniqueName;
		public const string UserId = "Id";
		public const string IsAdmin = "IsAdmin";
	}
}
