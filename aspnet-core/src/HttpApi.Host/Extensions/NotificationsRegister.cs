using HttpApi.Host.Hubs;
using HttpApi.Host.Services.NotificationInterfaces;

namespace HttpApi.Host.Extensions;

public static class NotificationsRegister
{
  public static WebApplicationBuilder RegisterNotificationImplementations ( this WebApplicationBuilder builder )
  {
    var services = builder.Services;
    services.AddScoped<IPlaceOpenedNotification, MainHub> ();
    services.AddScoped<IGameIsFinished, MainHub> ();
    return builder;
  }
}
