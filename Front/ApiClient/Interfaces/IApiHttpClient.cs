namespace Front.ApiClient.Interfaces
{
    public interface IApiHttpClient
    {
        Task<T> GetAsync<T>(string endpoint);
        Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data);
        Task<TResponse> PutAsync<TRequest,TResponse>(string endpoint, TRequest data);
        Task DeleteAsync(string endpoint);
        void UpdateAuthenticationToken(string token);

    }
}
