namespace Application.Components
{
    public interface IDataCacheService
    {
        Task Put(string key, object value, TimeSpan time);
        Task Put(string key, object value);
        Task<T> Get<T>(string key);
        Task<object> Get(string key);
    }
}
