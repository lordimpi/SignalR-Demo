namespace Business.Services.Contracts
{
    public interface IHttpService
    {
        Task<T?> GetAsync<T>(string url);

        Task<TResponse?> PostAsync<TRequest, TResponse>(string url, TRequest data);

        Task<bool> PutAsync<T>(string url, T data);

        Task<bool> DeleteAsync(string url);
    }
}