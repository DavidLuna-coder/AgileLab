using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Front.ApiClient.Interfaces;

namespace Front.ApiClient.Implementations
{
	public class ApiHttpClient : IApiHttpClient
    {
        private readonly string _backendBaseAddress;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
        public HttpClient Client { get; }
        public ApiHttpClient(IConfiguration configuration)
        {
            _backendBaseAddress = configuration["Backend:BaseAddress"]!;
            Client = new HttpClient()
            {
                BaseAddress = new Uri(_backendBaseAddress)
            };
        }

        public void UpdateAuthenticationToken(string token)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<T> GetAsync<T>(string endpoint)
        {
            var response = await Client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data)
        {
            string json = JsonSerializer.Serialize(data, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await Client.PostAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
        }

        public async Task<TResponse> PutAsync<TRequest,TResponse>(string endpoint, TRequest data)
        {
            var jsonContent = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
            var response = await Client.PutAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();
			return JsonSerializer.Deserialize<TResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
		}

        public async Task DeleteAsync(string endpoint)
        {
            var response = await Client.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
        }
	}
}
