using HttpApi.Host.Models.Games;

namespace HttpApi.Host.Services.GameServices;

public interface IGetGames
{
  Task<GameModel?> GetCurrentGameAsync ( string userId, CancellationToken cancellationToken );
  Task<IEnumerable<GameModel>> GetUserGamesAsync ( string userId, CancellationToken cancellationToken );
}