namespace Front.ApiClient
{
	public interface IAuthenticationService
	{
		public LocalUserInfo? User { get; }
		public Task Login();
		public Task Logout();
		public Task Initialize();
		public bool IsAuthenticated();
	}
}