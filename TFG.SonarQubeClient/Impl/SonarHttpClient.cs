using System.Text;
using System.Text.Json;

namespace TFG.SonarQubeClient.Impl
{
	public class SonarHttpClient : ISonarHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
        public SonarHttpClient(string baseUrl, string token)
		{

			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri(baseUrl)
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
		}

        public async Task<HttpResponseMessage> GetAsync(string endpoint, string version = null)
        {
            var response = await _httpClient.GetAsync(GetEndpointWithVersion(endpoint,version));
			EnsureSuccessStatusCode(response);
			return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, string? version = null)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions) ?? string.Empty;
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            endpoint = GetEndpointWithVersion(endpoint, version);

			var response = await _httpClient.PostAsync(endpoint, jsonContent);
			EnsureSuccessStatusCode(response);
			return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content, string? version = null)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(GetEndpointWithVersion(endpoint, version), jsonContent);
			EnsureSuccessStatusCode(response);
			return response;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, string? version = null)
        {
            var response = await _httpClient.DeleteAsync(GetEndpointWithVersion(endpoint, version));
			EnsureSuccessStatusCode(response);
			return response;
        }
        private static string GetEndpointWithVersion(string endpoint, string? version = null)
        {
            if (string.IsNullOrEmpty(version))
                return endpoint;

            return $"{version}/{endpoint}";
        }

		private static void EnsureSuccessStatusCode(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				var log = response.Content.ReadAsStringAsync().Result;
				throw new HttpRequestException(log);
			}
		}
	}
}
