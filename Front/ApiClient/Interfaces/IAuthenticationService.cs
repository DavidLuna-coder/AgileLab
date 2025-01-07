using Front.ApiClient.Implementations;
using Shared.DTOs;

namespace Front.ApiClient.Interfaces
{
    public interface IAuthenticationService
    {
        public LocalUserInfo? User { get; }
        public Task Login(LoginRequestDto requestDto);
        public Task Register(RegistrationDto requestDto);
		public Task Logout();
        public Task Initialize();
        public bool IsAuthenticated();
    }
}