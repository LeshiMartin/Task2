namespace HttpApi.Host.Services.CacheServices;

public interface ICacheService
{

  Task SetRecordAsync<T> ( string key, T data, TimeSpan? Expiry=null );
  Task<T?> GetRecordAsync<T> ( string key );
}