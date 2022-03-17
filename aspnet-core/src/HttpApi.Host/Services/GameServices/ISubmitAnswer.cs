using HttpApi.Host.DAL;
using HttpApi.Host.Entities;
using HttpApi.Host.Exceptions;
using HttpApi.Host.Models.Games;
using HttpApi.Host.Models.Players;
using HttpApi.Host.Services.PlayerServices;
using MediatR;

namespace HttpApi.Host.Services.GameServices;

public interface ISubmitAnswer
{
  Task<bool> SubmitAsync ( SubmitAnswerModel model, CancellationToken cancellationToken );
}


public class SubmitAnswer : ISubmitAnswer
{
  private readonly IGameRepo _gameRepo;
  private readonly IMediator _mediator;
  private readonly IPlayerService _playerService;
  private readonly IGameGenerator _gameGenerator;

  public SubmitAnswer ( IGameRepo gameRepo,
    IMediator mediator,
    IPlayerService playerService,
    IGameGenerator gameGenerator )
  {
    _gameRepo = gameRepo;
    _mediator = mediator;
    _playerService = playerService;
    _gameGenerator = gameGenerator;
  }
  public async Task<bool> SubmitAsync ( SubmitAnswerModel model, CancellationToken cancellationToken )
  {
    var (gameId, submitAnswer, userId) = model;
    var game = await _gameRepo
      .GetGameAsync (gameId, cancellationToken);
    if ( game.IsFinished )
      throw new GameFinishedException ();

    var userGame = await _gameRepo
      .GetUserGameAsync (userId, gameId, cancellationToken);
    await UpdateUserGame (cancellationToken,
      userGame!, submitAnswer, game);

    if ( AnswerIsCorrect (userGame!) )
    {
      await SetNotAnsweredToMissedAsync (cancellationToken, gameId);
      await SetGameToFinished (game, cancellationToken);
      return true;
    }

    var players = await _playerService.InGamePlayersAsync ();
    var inCorrectGames = await _gameRepo.InCorrectlyAnsweredGamesAsync (gameId, cancellationToken);
    if ( AllPlayersHaveSubmittedAnswers (inCorrectGames, players) )
      await SetGameToFinished (game, cancellationToken);


    return AnswerIsCorrect (userGame!);

  }

  private async Task UpdateUserGame ( CancellationToken cancellationToken,
    UserGame userGame,
    string submitAnswer,
    Game game )
  {
    userGame.ProposedAnswer = submitAnswer;
    userGame.UserGameStatus = (int) UserGameStatusHelper
      .GetStatus (submitAnswer, game);
    await _gameRepo.UpdateUserGameAsync (userGame, cancellationToken);
  }

  private static bool AllPlayersHaveSubmittedAnswers ( IEnumerable<UserGame> inCorrectGames,
    IReadOnlyCollection<Player> players )
  {
    return inCorrectGames.Where (x => players
       .Select (c => c.UserId)
       .Contains (x.UserId))
      .All (x => x.UserGameStatus == (int) UserGameStatuses.InCorrect);
  }

  private async Task SetGameToFinished ( Game game, CancellationToken cancellationToken )
  {
    game.IsFinished = true;
    await _gameRepo.UpdateGameAsync (game, cancellationToken);
    await _gameGenerator.GenerateGame (cancellationToken);
    await _mediator.Publish (new GameFinishedNotificationRequest (), cancellationToken);
  }

  private async Task SetNotAnsweredToMissedAsync ( CancellationToken cancellationToken, int gameId )
  {
    var notAnsweredGames = await _gameRepo.NotAnsweredGamesAsync (gameId, cancellationToken);
    foreach ( var notAnsweredGame in notAnsweredGames )
    {
      notAnsweredGame.UserGameStatus = (int) UserGameStatuses.Missed;
      await _gameRepo.UpdateUserGameAsync (notAnsweredGame, cancellationToken);
    }
  }

  private static bool AnswerIsCorrect ( UserGame userGame )
  {
    return UserGameStatusHelper.IsCorrect (userGame.UserGameStatus);
  }
}