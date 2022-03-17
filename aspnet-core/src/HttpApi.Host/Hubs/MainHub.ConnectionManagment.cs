using HttpApi.Host.Models.Players;

namespace HttpApi.Host.Hubs;

public partial class MainHub
{

  protected virtual string UserId () => Context.UserIdentifier ?? "";
  protected virtual string ConnectionId () => Context.ConnectionId;

  public override async Task OnConnectedAsync ()
  {
    var player = new Player (UserId (), ConnectionId ());
    await _playerService
      .PlayerConnectedAsync (player);
  }


  public async Task OnDisconnectedAsync ( Exception? exception )
  {
    _logger.LogError (exception, "On disconnected called for user :{userId}", UserId ());
    await _playerService
      .PlayerDisconnectedAsync (UserId ());
  }


}
