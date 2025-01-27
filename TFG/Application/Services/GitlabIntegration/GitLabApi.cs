using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace TFG.Application.Services.GitlabIntegration
{
    public class GitLabApi
    {
        private readonly HttpClient _httpClient;
        private readonly string GITLAB_BASE_ADDRESS;
        private readonly string GITLAB_API_KEY;
        private JsonSerializerOptions _serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower };

		public GitLabApi(IConfiguration configuration)
        {
            GITLAB_BASE_ADDRESS = configuration["GitLab:GitLabBaseAddress"];
            GITLAB_API_KEY = configuration["GitLab:GitLabApiKey"];
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(GITLAB_BASE_ADDRESS)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", GITLAB_API_KEY);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				var log = await response.Content.ReadAsStringAsync();
				throw new Exception(log);
			}
			return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, jsonContent);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				var log = await response.Content.ReadAsStringAsync();
				throw new Exception(log);
			}
			return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, jsonContent);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				var log = await response.Content.ReadAsStringAsync();
				throw new Exception(log);
			}
			return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var response = await _httpClient.DeleteAsync(endpoint);
			try
			{
				response.EnsureSuccessStatusCode();
			}
			catch
			{
				var log = await response.Content.ReadAsStringAsync();
				throw new Exception(log);
			}
			return response;
        }
    }
}
