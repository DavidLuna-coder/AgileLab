using Blazored.LocalStorage;
using Shared.Utils.DateTimeProvider;

namespace Front.ApiClient
{
	public class AuthenticationService(ILocalStorageService localStorageService, IDateTimeProvider dateTimeProvider) : IAuthenticationService
	{
		public LocalUserInfo? User { get; private set; }
		private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
		private readonly ILocalStorageService _localstorageService = localStorageService;
		public async Task Initialize()
		{
			User = await _localstorageService.GetItemAsync<LocalUserInfo>("user");
		}
		public async Task Login()
		{
			LocalUserInfo userInfo = new();
			await _localstorageService.SetItemAsync("user", userInfo);
		}

		public async Task Logout()
		{
			await _localstorageService.RemoveItemAsync("user");
		}

		public bool IsAuthenticated()
		{
			return User != null && User.ExpirationDate.ToLocalTime() > _dateTimeProvider.Now;
		}
	}
}
