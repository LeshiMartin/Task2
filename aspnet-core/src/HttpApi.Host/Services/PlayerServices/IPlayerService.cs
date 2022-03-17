using HttpApi.Host.Models.Players;

namespace HttpApi.Host.Services.PlayerServices;

public interface IPlayerService
{
  Task<HashSet<Player>> InGamePlayersAsync ();
  Task<HashSet<Player>> WaitingPlayersAsync ();
  Task PlayerConnectedAsync ( Player player );
  Task PlayerDisconnectedAsync ( string userId );
  Task<bool> CheckForOpenPositionAsync ();
  Task<bool> GetInGameAsync ( Player player );

}