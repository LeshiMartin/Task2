using System.Text.Json;
using StackExchange.Redis;

namespace HttpApi.Host.Services.CacheServices;

public class CacheService : ICacheService
{

  private readonly IDatabase _db;

  public CacheService ( IConnectionMultiplexer connectionMultiplexer )
  {
    _db = connectionMultiplexer.GetDatabase ();
  }
  public async Task SetRecordAsync<T> ( string key, T data, TimeSpan? Expiry = null )
  {

    var valStr = JsonSerializer.Serialize<T> (data);
    await _db.StringSetAsync (key, valStr, Expiry ?? TimeSpan.FromHours (SystemConstants.REDIS_EXPIRY_HOURS));
  }

  public async Task<T?> GetRecordAsync<T> ( string key )
  {
    var valStr = await GetStringAsync (key);
    return string.IsNullOrEmpty (valStr) ?
      default :
      JsonSerializer.Deserialize<T> (valStr)!;
  }

  private async Task<string> GetStringAsync ( string key )
  {
    return await _db.StringGetAsync (key);
  }
}
