using System.Text;
using System.Text.Json;
using TFG.GoRaceClient.Dtos;

namespace TFG.GoRaceClient
{
	public class GoRaceHttpClient : IGoRaceHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);
		public GoRaceHttpClient(string baseUrl, string token)
		{
			if (string.IsNullOrEmpty(baseUrl)) baseUrl = "https://api-gorace.uca.es";
			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri(baseUrl)
			};
			_httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
		}

		private static void EnsureSuccessStatusCode(HttpResponseMessage response)
		{
			if (!response.IsSuccessStatusCode)
			{
				var log = response.Content.ReadAsStringAsync().Result;
				throw new HttpRequestException(log);
			}
		}

		public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
		{
			var response = await _httpClient.DeleteAsync(endpoint);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> GetAsync(string endpoint)
		{
			var response = await _httpClient.GetAsync(endpoint);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content)
		{
			return await PostAsync(endpoint, content, null);
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, JsonSerializerOptions? options)
		{
			var serializerOptions = options ?? _serializerOptions;
			string json = JsonSerializer.Serialize(content, serializerOptions) ?? string.Empty;
			var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await _httpClient.PostAsync(endpoint, jsonContent);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> PostAsync(string endpoint)
		{
			var response = await _httpClient.PostAsync(endpoint, null);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content)
		{
			string json = JsonSerializer.Serialize(content, _serializerOptions);
			var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(endpoint, jsonContent);
			EnsureSuccessStatusCode(response);
			return response;
		}
	}
}
