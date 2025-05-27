namespace TFG.Application.Security
{
	public interface IUserInfo
	{
		string Email { get; }
		string Username { get; }
		string UserId { get; }
		bool IsAdmin { get; }
	}
}
