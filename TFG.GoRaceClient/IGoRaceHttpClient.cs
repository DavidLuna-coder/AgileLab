namespace TFG.GoRaceClient
{
	public interface IGoRaceHttpClient
	{
		Task<HttpResponseMessage> GetAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, System.Text.Json.JsonSerializerOptions? options);
		Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> DeleteAsync(string endpoint);
	}
}
