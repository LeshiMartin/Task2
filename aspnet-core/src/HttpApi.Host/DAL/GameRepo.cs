using HttpApi.Host.Entities;
using Microsoft.EntityFrameworkCore;

namespace HttpApi.Host.DAL;

public class GameRepo : IGameRepo
{
  private readonly ApplicationDbContext _dbContext;

  public GameRepo ( IDbContextFactory<ApplicationDbContext> factory )
  {
    _dbContext = factory.CreateDbContext ();
  }
  public async Task<int> InsertGameAsync ( Game game, CancellationToken cancellationToken )
  {
    if ( game == null )
      throw new ArgumentNullException (nameof (game));
    await _dbContext
      .Games.AddAsync (game, cancellationToken);
    return await _dbContext.SaveChangesAsync (cancellationToken);
  }

  public Task<int> UpdateGameAsync ( Game game, CancellationToken cancellationToken )
  {
    if ( game == null )
      throw new ArgumentNullException (nameof (game));
    _dbContext.Games.Update (game);
    return _dbContext.SaveChangesAsync (cancellationToken);
  }

  public Task<Game> GetGameAsync ( int id, CancellationToken cancellationToken )
    => _dbContext.Games.SingleAsync (x => x.Id == id, cancellationToken);

  public Task<Game?> GetCurrentGameAsync ( CancellationToken cancellationToken )
    => _dbContext.Games.FirstOrDefaultAsync (x => !x.IsFinished, cancellationToken);



  public async Task<int> InsertUserGameAsync ( UserGame userGame, CancellationToken cancellationToken )
  {
    if ( userGame == null )
      throw new ArgumentNullException (nameof (userGame));
    await _dbContext
      .UserGames.AddAsync (userGame, cancellationToken);
    return await _dbContext.SaveChangesAsync (cancellationToken);
  }

  public Task<int> UpdateUserGameAsync ( UserGame userGame, CancellationToken cancellationToken )
  {
    if ( userGame == null )
      throw new ArgumentNullException (nameof (userGame));
    _dbContext.UserGames.Update (userGame);
    return _dbContext.SaveChangesAsync (cancellationToken);
  }

  public async Task<IEnumerable<UserGame>> GetUserGamesAsync ( string userId, CancellationToken cancellationToken )
  {
    if ( userId == null )
      throw new ArgumentNullException (nameof (userId));
    return await _dbContext
      .UserGames
      .Include (x => x.Game)
      .Where (x => x.UserId == userId)
      .ToArrayAsync (cancellationToken);
  }

  public async Task<IEnumerable<UserGame>> NotAnsweredGamesAsync ( int gameId, CancellationToken cancellationToken )
  => await _dbContext.UserGames
    .Where (x => x.GameId == gameId &&
                 x.UserGameStatus == (int) UserGameStatuses.NotAnswered)
    .ToArrayAsync (cancellationToken);

  public async Task<IEnumerable<UserGame>> InCorrectlyAnsweredGamesAsync ( int gameId, CancellationToken cancellationToken )
    => await _dbContext.UserGames
      .Where (x => x.GameId == gameId &&
                   x.UserGameStatus == (int) UserGameStatuses.InCorrect)
      .ToArrayAsync (cancellationToken);

  public Task<UserGame?> GetUserGameAsync ( string userId, int gameId, CancellationToken cancellationToken )
  {
    if ( userId == null )
      throw new ArgumentNullException (nameof (userId));
    return _dbContext.UserGames.
      SingleOrDefaultAsync (x => x.UserId == userId && x.GameId == gameId, cancellationToken);
  }
}