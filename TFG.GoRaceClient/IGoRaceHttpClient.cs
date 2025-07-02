namespace TFG.GoRaceClient
{
	public interface IGoRaceHttpClient
	{
		Task<HttpResponseMessage> GetAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync(string endpoint);
		Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content);
		Task<HttpResponseMessage> DeleteAsync(string endpoint);
	}
}
