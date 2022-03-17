using HttpApi.Host.Services.GameServices;
using HttpApi.Host.Services.PlayerServices;

namespace HttpApi.Host.Extensions;

public static class GameServicesRegistration
{
  public static void RegisterGameServices ( this WebApplicationBuilder builder )
  {
    var services = builder.Services;
    services.AddScoped<IGameGenerator, GameGenerator> ();
    services.AddScoped<IPlayerService, PlayerService> ();
    services.AddScoped<ISubmitAnswer, SubmitAnswer> ();
    services.AddScoped<IGetGames, GetGames> ();
  }
}
