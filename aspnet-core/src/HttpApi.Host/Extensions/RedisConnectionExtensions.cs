using HttpApi.Host.Services.CacheServices;
using StackExchange.Redis;

namespace HttpApi.Host.Extensions;

public static class RedisConnectionExtensions
{
  public static IServiceCollection RegisterRedis ( this WebApplicationBuilder builder )
  {
    builder.Services.AddSingleton<IConnectionMultiplexer> (b =>
      ConnectionMultiplexer.Connect ($"localhost:{builder.Configuration[ "Redis_Port" ]}"));
    builder.Services.AddSingleton<ICacheService, CacheService> ();
    return builder.Services;
  }
}
