using AutoMapper;
using HttpApi.Host.DAL;
using HttpApi.Host.Entities;
using HttpApi.Host.Models.Games;

namespace HttpApi.Host.Services.GameServices;

public interface IGetGames
{
  Task<GameModel> GetCurrentGameAsync ( string userId, CancellationToken cancellationToken );
  Task<IEnumerable<GameModel>> GetUserGamesAsync ( string userId, CancellationToken cancellationToken );
}


public class GetGames : IGetGames
{
  private readonly IGameRepo _gameRepo;
  private readonly IGameGenerator _gameGenerator;
  private readonly IMapper _mapper;

  public GetGames ( IGameRepo gameRepo, IGameGenerator gameGenerator, IMapper mapper )
  {
    _gameRepo = gameRepo;
    _gameGenerator = gameGenerator;
    _mapper = mapper;
  }
  public async Task<GameModel> GetCurrentGameAsync ( string userId, CancellationToken cancellationToken )
  {
    var currentGame = await _gameRepo.GetCurrentGameAsync (cancellationToken) ??
                      await _gameGenerator.GenerateGame (cancellationToken);

    var userGame = await _gameRepo.GetUserGameAsync (userId, currentGame.Id, cancellationToken) ??
                   await CreateUserGame (userId, cancellationToken, currentGame);
    return _mapper.Map<UserGame, GameModel> (userGame);
  }

  private async Task<UserGame> CreateUserGame ( string userId, CancellationToken cancellationToken, Game currentGame )
  {
    var userGame = new UserGame ()
    {
      GameId = currentGame.Id,
      UserId = userId
    };
    await _gameRepo.InsertUserGameAsync (userGame, cancellationToken);
    return userGame;
  }

  public async Task<IEnumerable<GameModel>> GetUserGamesAsync ( string userId, CancellationToken cancellationToken )
  {
    var userGames = await _gameRepo.GetUserGamesAsync(userId, cancellationToken);
    return userGames.Select(x => _mapper.Map<UserGame, GameModel>(x));
  }
}
