using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace TFG.Application.Services.OpenProjectIntegration
{
    public class OpenProjectApi
    {
        private readonly HttpClient _httpClient;
        private readonly string OPENPROJECT_BASE_ADDRESS;
        private readonly string OPENPROJECT_API_KEY;
        private JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

        public OpenProjectApi(IConfiguration configuration)
        {
            OPENPROJECT_BASE_ADDRESS = configuration["OpenProject:OpenProjectBaseAddress"];
            OPENPROJECT_API_KEY = configuration["OpenProject:OpenProjectApiKey"];
            var byteArray = new ASCIIEncoding().GetBytes($"apikey:{OPENPROJECT_API_KEY}");

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(OPENPROJECT_BASE_ADDRESS)
            };
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content)
        {
            string json = JsonSerializer.Serialize(content, _serializerOptions);
            var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(endpoint, jsonContent);
            response.EnsureSuccessStatusCode();
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {

            var response = await _httpClient.DeleteAsync(endpoint);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
