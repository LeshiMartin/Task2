using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HttpApi.Host.Hubs
{
  [Authorize]
  public class MainHub : Hub
  {
  }
}
