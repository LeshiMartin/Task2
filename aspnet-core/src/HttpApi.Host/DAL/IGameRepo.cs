using HttpApi.Host.Entities;

namespace HttpApi.Host.DAL;

public interface IGameRepo
{
  Task<int> InsertGameAsync ( Game game, CancellationToken cancellationToken );
  Task<int> UpdateGameAsync ( Game game, CancellationToken cancellationToken );
  Task<Game> GetGameAsync ( int id, CancellationToken cancellationToken );
  Task<Game?> GetCurrentGameAsync(CancellationToken cancellationToken);
  Task<int> InsertUserGameAsync ( UserGame userGame, CancellationToken cancellationToken );
  Task<int> UpdateUserGameAsync ( UserGame userGame, CancellationToken cancellationToken );
  Task<IEnumerable<UserGame>> GetUserGamesAsync ( string userId, CancellationToken cancellationToken );
  Task<IEnumerable<UserGame>> NotAnsweredGamesAsync ( int gameId, CancellationToken cancellationToken );
  Task<IEnumerable<UserGame>> InCorrectlyAnsweredGamesAsync ( int gameId, CancellationToken cancellationToken );
  Task<UserGame?> GetUserGameAsync ( string userId, int gameId, CancellationToken cancellationToken );
}