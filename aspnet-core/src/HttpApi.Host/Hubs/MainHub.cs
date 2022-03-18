using HttpApi.Host.Exceptions;
using HttpApi.Host.Models.Games;
using HttpApi.Host.Models.Players;
using HttpApi.Host.Services.GameServices;
using HttpApi.Host.Services.NotificationInterfaces;
using HttpApi.Host.Services.PlayerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace HttpApi.Host.Hubs
{
  [Authorize]
  public partial class MainHub : Hub, IPlaceOpenedNotification, IGameIsFinished, IPlayerJoined
  {

    private const string PLACE_OPENED = "placeOpened";
    private const string GAME_FINISHED = "gameFinished";
    private const string PLAYER_JOINED = "playerJoined";

    private readonly IHubContext<MainHub> _hubContext;
    private readonly ILogger<MainHub> _logger;
    private readonly IGetGames _getGames;
    private readonly ISubmitAnswer _submitAnswer;
    private readonly IPlayerService _playerService;
    private readonly CancellationToken _cancellationToken;

    public MainHub ( IHubContext<MainHub> hubContext,
      ILogger<MainHub> logger,
      IGetGames getGames,
      ISubmitAnswer submitAnswer,
      IPlayerService playerService )
    {
      _hubContext = hubContext;
      _logger = logger;
      _getGames = getGames;
      _submitAnswer = submitAnswer;
      _playerService = playerService;
      _cancellationToken = new CancellationTokenSource (TimeSpan.FromMinutes (1)).Token;
    }

    public async Task<bool> CheckForOpenPlace ()
    {

      try
      {
        _logger.LogInformation ("MainHub => CheckForOpenPlace Called from user : {userId}", UserId ());
        return await _playerService.CheckForOpenPositionAsync ();
      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, exc.Message);
        throw;
      }
    }

    public async Task<bool> GetInGame ()
    {

      try
      {
        _logger.LogInformation ("MainHub => GetInGame Called from user : {userId}", UserId ());
        return await _playerService
          .GetInGameAsync (new Player (UserId (), ConnectionId ()));
      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, exc.Message);
        throw;
      }
    }

    public async Task<IEnumerable<GameModel>> GetUserGames ()
    {

      try
      {
        _logger.LogInformation ("MainHub => GetUserGames Called from user : {userId}", UserId ());
        return await _getGames.GetUserGamesAsync (UserId (), _cancellationToken);
      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, exc.Message);
        throw;
      }
    }

    public async Task<GameModel?> GetCurrentGame ()
    {
      try
      {
        _logger.LogInformation ("MainHub => GetCurrentGame Called from user : {userId}", UserId ());
        return await _getGames.GetCurrentGameAsync (UserId (), _cancellationToken);

      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, "{Message}", exc.Message);
        throw;
      }
    }

    public async Task LeaveGame ()
    {

      try
      {
        _logger.LogInformation ("MainHub => LeaveGame Called from user : {userId}", UserId ());
        await _playerService.PlayerDisconnectedAsync (UserId ());
      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, exc.Message);

      }
    }

    public async Task<int> SubmitAnswer ( string answer, int gameId )
    {
      try
      {
        _logger.LogInformation ("MainHub => SubmitAnswer Called from user : {userId}", UserId ());
        var response =
          await _submitAnswer.SubmitAsync(new SubmitAnswerModel(gameId, answer, UserId()), _cancellationToken);
        return response ? 1 : 0;
      }
      catch ( GameFinishedException exc )
      {
        _logger.LogError (exc, "{message}", exc.Message);
        return -1;
      }
      catch ( Exception exc )
      {
        _logger.LogError (exc, "{Message}", exc.Message);
        return -100;
      }
    }

    public async Task<string> GetAttendingsCount () =>
      $"There are currently{(await _playerService.InGamePlayersAsync ()).Count} users online";

    public async Task NotifyPlaceOpened ()
    {
      await _hubContext.Clients
        .All.SendAsync (PLACE_OPENED,_cancellationToken);
    }

    public async Task NotifyGameIsFinished ()
    {
      var inGamePlayers = await _playerService.InGamePlayersAsync ();
      await _hubContext.Clients
        .Clients (inGamePlayers.Select (x => x.ConnectionId))
        .SendAsync (GAME_FINISHED);
    }

    public async Task PlayerJoinedAsync ()
    {
      var inGamePlayers = await _playerService.InGamePlayersAsync ();
      await _hubContext.Clients
        .Clients (inGamePlayers.Select (x => x.ConnectionId))
        .SendAsync (PLAYER_JOINED, _cancellationToken);
    }
  }
}
