namespace TFG.OpenProjectClient
{
	public interface IOpenProjectHttpClient
	{
		Task<HttpResponseMessage> GetAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> PutAsync(string endpoint);
		Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> DeleteAsync(string endpoint);
		Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T content);
	}
}
