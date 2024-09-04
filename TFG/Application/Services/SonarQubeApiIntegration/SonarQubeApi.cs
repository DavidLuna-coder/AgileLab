using System.Text;
using System.Text.Json;

namespace TFG.Application.Services.SonarQubeIntegration
{
    public class SonarQubeApi
    {
        private readonly HttpClient _httpClient;
        private readonly string SONARQUBE_BASE_ADDRESS;
        private readonly string SONARQUBE_API_KEY;
        private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

        public SonarQubeApi(IConfiguration configuration)
        {
            SONARQUBE_BASE_ADDRESS = configuration["SonarQube:SonarQubeBaseAddress"];
            SONARQUBE_API_KEY = configuration["SonarQube:SonarQubeApiKey"];
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(SONARQUBE_BASE_ADDRESS)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", SONARQUBE_API_KEY);
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint, string version = null)
        {
            var response = await _httpClient.GetAsync(GetEndpointWithVersion(endpoint,version));
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, string version = null)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
            endpoint = GetEndpointWithVersion(endpoint, version);

			var response = await _httpClient.PostAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content, string version = null)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(GetEndpointWithVersion(endpoint, version), jsonContent);
            response.EnsureSuccessStatusCode();
            return response;
        }
        public async Task<HttpResponseMessage> DeleteAsync(string endpoint, string version = null)
        {
            var response = await _httpClient.DeleteAsync(GetEndpointWithVersion(endpoint, version));
            response.EnsureSuccessStatusCode();
            return response;
        }
        private string GetEndpointWithVersion(string endpoint, string version = null)
        {
            if (string.IsNullOrEmpty(version))
                return endpoint;

            return $"{version}/{endpoint}";
        }
    }
}
