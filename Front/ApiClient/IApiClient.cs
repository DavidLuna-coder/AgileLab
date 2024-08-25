using Shared.DTOs;

namespace Front.ApiClient
{
	public interface IApiClient
	{
		public Task Login(LoginRequestDto user);
		public Task Register(RegistrationDto registration);
	}
}
