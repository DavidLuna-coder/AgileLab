using TFG.Application.Security;

namespace TFG.Application.Dtos
{
	public class UserInfo : IUserInfo
	{
		public string Email { get; set; }
		public string Username { get; set; }
		public string UserId { get; set; }
		public bool IsAdmin { get; set; }
	}
}
