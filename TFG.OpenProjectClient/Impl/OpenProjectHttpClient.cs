using System;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;

namespace TFG.OpenProjectClient.Impl
{
	public class OpenProjectHttpClient : IOpenProjectHttpClient
	{
		private readonly HttpClient _httpClient;
		private readonly JsonSerializerOptions _serializerOptions = new(JsonSerializerDefaults.Web);

		public OpenProjectHttpClient(string baseUrl, string apikey)
		{
			if (string.IsNullOrEmpty(baseUrl)) throw new ArgumentException($"'{nameof(baseUrl)}' cannot be null or empty.", nameof(baseUrl));
			if (string.IsNullOrEmpty(apikey)) throw new ArgumentException($"'{nameof(apikey)}' cannot be null or empty.", nameof(apikey));
			var byteArray = new ASCIIEncoding().GetBytes($"apikey:{apikey}");

			_httpClient = new HttpClient()
			{
				BaseAddress = new Uri($"{baseUrl}/api/v3/")
			};
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
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

		public async Task<HttpResponseMessage> PostAsync(string endpoint)
		{
			var response = await _httpClient.PostAsync(endpoint, null);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content)
		{
			string json = JsonSerializer.Serialize(content, _serializerOptions);
			var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync(endpoint, jsonContent);
			EnsureSuccessStatusCode(response);
			return response;
		}

		public async Task<HttpResponseMessage> PutAsync(string endpoint)
		{
			var response = await _httpClient.PutAsync(endpoint, null);
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
