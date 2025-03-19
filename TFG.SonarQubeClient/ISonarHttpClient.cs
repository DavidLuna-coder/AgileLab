namespace TFG.SonarQubeClient
{
    public interface ISonarHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string endpoint, string version = null);
        Task<HttpResponseMessage> PostAsync<T>(string endpoint, T content, string? version = null);
        Task<HttpResponseMessage> PutAsync<T>(string endpoint, T content, string? version = null);
        Task<HttpResponseMessage> DeleteAsync(string endpoint, string? version = null);
    }
}
