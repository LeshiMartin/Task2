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
  public partial class MainHub : Hub, IPlaceOpenedNotification, IGameIsFinished
  {

    private const string PLACE_OPENED = "placeOpened";
    private const string GAME_FINISHED = "gameFinished";

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

    public async Task<bool> CheckForOpenPlace () => await _playerService.CheckForOpenPositionAsync ();

    public async Task<bool> GetInGame () => await _playerService.GetInGameAsync (new Player (UserId (), ConnectionId ()));

    public async Task<IEnumerable<GameModel>> GetUserGames () =>
      await _getGames.GetUserGamesAsync (UserId (), _cancellationToken);

    public async Task<GameModel> GetCurrentGame () => await _getGames.GetCurrentGameAsync (UserId (), _cancellationToken);

    public async Task<int> SubmitAnswer ( string answer, int gameId )
    {
      try
      {
        var response =
          await _submitAnswer
            .SubmitAsync (new SubmitAnswerModel (gameId, answer, UserId ()), _cancellationToken);
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

    public async Task NotifyPlaceOpened ()
    {
      var usersWaiting = await _playerService.WaitingPlayersAsync ();
      await _hubContext.Clients
        .Clients (usersWaiting.Select (x => x.ConnectionId))
        .SendAsync (PLACE_OPENED);
    }

    public async Task NotifyGameIsFinished ()
    {
      var inGamePlayers = await _playerService.InGamePlayersAsync ();
      await _hubContext.Clients
        .Clients (inGamePlayers.Select (x => x.ConnectionId))
        .SendAsync (GAME_FINISHED);
    }
  }
}
