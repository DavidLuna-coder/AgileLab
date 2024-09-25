using Blazored.LocalStorage;
using Front.ApiClient.Interfaces;
using Shared.DTOs;
using Shared.Utils.DateTimeProvider;

namespace Front.ApiClient.Implementations
{
    public class AuthenticationService(ILocalStorageService localStorageService, IDateTimeProvider dateTimeProvider, IApiHttpClient apiHttpClient) : IAuthenticationService
    {
        public LocalUserInfo? User { get; private set; }
        private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
        private readonly ILocalStorageService _localstorageService = localStorageService;
        private readonly IApiHttpClient _httpClient = apiHttpClient;
        public async Task Initialize()
        {
            if (User != null) return;

            User = await _localstorageService.GetItemAsync<LocalUserInfo>("user");
            if (User != null)
                _httpClient.UpdateAuthenticationToken(User.Token);
        }
        public async Task Login(LoginRequestDto request)
        {
            User = await _httpClient.PostAsync<LoginRequestDto, LocalUserInfo>("/api/Authentication/login", request);
            await _localstorageService.SetItemAsync("user", User);
            _httpClient.UpdateAuthenticationToken(User.Token);

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
