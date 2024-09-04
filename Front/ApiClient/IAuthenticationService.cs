using Shared.DTOs;

namespace Front.ApiClient
{
	public interface IAuthenticationService
	{
		public LocalUserInfo? User { get; }
		public Task Login(LoginRequestDto requestDto);
		public Task Logout();
		public Task Initialize();
		public bool IsAuthenticated();
	}
}