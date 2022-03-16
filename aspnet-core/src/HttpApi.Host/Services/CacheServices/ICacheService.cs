namespace HttpApi.Host.Services.CacheServices;

public interface ICacheService
{

  Task SetRecordAsync<T> ( string key, T data, TimeSpan? Expiry );
  Task<T> GetRecordAsync<T> ( string key );
}