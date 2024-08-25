using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Front.ApiClient
{
	public class ApiHttpClient : IApiHttpClient
	{
		public HttpClient Client { get; }
		private readonly ILocalStorageService _localStorageService;
		public ApiHttpClient()
		{
			Client = new HttpClient()
			{
				BaseAddress = new Uri("localhost:80/api/"), //Añadir config a eso
			};

		}

		public void UpdateAuthenticationToken(string token)
		{
			Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			throw new NotImplementedException();
		}
	}
}
