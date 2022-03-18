using HttpApi.Host.Models.Players;
using HttpApi.Host.Services.CacheServices;
using HttpApi.Host.Services.GameServices;
using MediatR;

namespace HttpApi.Host.Services.PlayerServices;

public class PlayerService : IPlayerService
{
  private const string USERS_IN_GAME_KEY = "USERS_IN_GAME";
  private const string USERS_WAITING_KEY = "USERS_WAITING";

  private readonly ICacheService _cacheService;
  private readonly IMediator _mediator;


  public PlayerService ( ICacheService cacheService, IMediator mediator )
  {
    _cacheService = cacheService;
    _mediator = mediator;

  }
  public async Task<HashSet<Player>> InGamePlayersAsync ()
  {
    return (await _cacheService.GetRecordAsync<HashSet<Player>> (USERS_IN_GAME_KEY) ?? new HashSet<Player> ());
  }

  public async Task<HashSet<Player>> WaitingPlayersAsync ()
  {
    return (await _cacheService.GetRecordAsync<HashSet<Player>> (USERS_WAITING_KEY) ?? new HashSet<Player> ());
  }

  public async Task PlayerConnectedAsync ( Player player )
  {
    var waitingList = await WaitingPlayersAsync ();
    if ( waitingList.Any (x => x.UserId == player.UserId) )
      waitingList = waitingList.Where (x => x.UserId != player.UserId).ToHashSet ();

    waitingList.Add (player);
    await _cacheService.SetRecordAsync (USERS_WAITING_KEY, waitingList);
  }

  public async Task PlayerDisconnectedAsync ( string userId )
  {
    var inGamePlayers = await InGamePlayersAsync ();
    if ( inGamePlayers.Any (x => x.UserId == userId) )
      await InGamePlayerLeave (userId, inGamePlayers);

    await LeaveWaitingList (userId);

  }

  private async Task LeaveWaitingList ( string userId )
  {
    var waitingPlayers = await WaitingPlayersAsync ();
    await _cacheService.SetRecordAsync (USERS_WAITING_KEY,
      waitingPlayers.Where (x => x.UserId != userId));
  }

  private async Task InGamePlayerLeave ( string userId, IEnumerable<Player> inGamePlayers )
  {
    await _cacheService.SetRecordAsync (USERS_IN_GAME_KEY,
      inGamePlayers.Where (x => x.UserId != userId));
    await _mediator.Publish (new PlaceOpenedNotificationRequest ());
  }

  public async Task<bool> CheckForOpenPositionAsync ()
  {
    return (await InGamePlayersAsync ()).Count < SystemConstants.GAME_SEATS_NR;
  }

  public async Task<bool> GetInGameAsync ( Player player )
  {
    if ( !await CheckForOpenPositionAsync () )
      return false;

    var inGameUsers = (await InGamePlayersAsync ()).Where (x => x.UserId != player.UserId)
      .ToHashSet ();
    inGameUsers.Add (player);
    await _cacheService.SetRecordAsync (USERS_IN_GAME_KEY, inGameUsers);
    await _mediator.Publish (new PlayerJoinedNotificationRequest ());
    await LeaveWaitingList (player.UserId);
    return true;

  }
}