using Shared.DTOs;

namespace Front.ApiClient
{
	public class ApiClient : IApiClient
	{
		public Task Login(LoginRequestDto user)
		{
			throw new NotImplementedException();
		}

		public Task Register(RegistrationDto registration)
		{
			throw new NotImplementedException();
		}
	}
}
