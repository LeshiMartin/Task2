using HttpApi.Host.DAL;
using HttpApi.Host.Entities;
using HttpApi.Host.Extensions;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo ("HttpApi.Host.Tests")]
namespace HttpApi.Host.Services.GameServices;

public class GameGenerator : IGameGenerator
{
  private readonly IGameRepo _gameRepo;
  private readonly Random _random;

  public GameGenerator ( IGameRepo gameRepo )
  {
    _gameRepo = gameRepo;
    _random = new Random ();
  }
  private static readonly IDictionary<int, string> _operationSigns =
    new Dictionary<int, string>
    {
    { 0, "+" },
    { 1, "-" },
    { 2, "*" },
    { 3, "/" },
  };

  private static readonly IDictionary<string, Func<int, int, double>> _operations =
    new Dictionary<string, Func<int, int, double>>
    {
      { "+", Add },
      { "-", Subtract },
      { "*", Multiply },
      { "/", Divide },
    };
  private static double Add ( int x, int y )
    => Math.Round ((double) (x + y), 2, MidpointRounding.AwayFromZero);
  private static double Subtract ( int x, int y )
    => Math.Round ((double) (x - y), 2, MidpointRounding.AwayFromZero);
  private static double Multiply ( int x, int y )
    => Math.Round ((double) (x * y), 2, MidpointRounding.AwayFromZero);
  private static double Divide ( int x, int y )
    => Math.Round (x / (double) y, 2);

  private static readonly IDictionary<bool, Func<double, double>> _solutionSuggestion =
    new Dictionary<bool, Func<double, double>>
    {
      { true, x => x },
      { false, _ => new Random().Next(short.MinValue, short.MaxValue) }
    };



  public async Task<Game> GenerateGame ( CancellationToken cancellationToken )
  {
    var x = GenerateRandom16BitNumber ();
    var y = GenerateRandom16BitNumber ();
    var operatorSign = GenerateOperationSign ();
    var solution = GenerateSolutionValue (x, y, operatorSign);
    var solutionSuggestion = _solutionSuggestion[ _random.Next (1, 3) % 2 == 0 ] (solution);
    var answer = GenerateCorrectAnswer (solution, solutionSuggestion);

    var game = new Game ()
    {
      AnswerValue = answer,
      Condition = $"{x} {operatorSign} {y} = {solutionSuggestion}",
      CorrectAnswer = solution
    };
    await _gameRepo.InsertGameAsync (game, cancellationToken);
    return game;
  }
  private int GenerateRandom16BitNumber ()
    => _random.Next (short.MinValue, short.MaxValue);

  internal string GenerateOperationSign ()
    => _operationSigns[ _random.Next (0, 4) ];

  private static string GenerateCorrectAnswer ( double solution, double res )
   => solution.IsEqualTo (res) ? "Yes" : "No";

  internal static double GenerateSolutionValue ( int x, int y, string operationSign )
    => _operations[ operationSign ] (x, y);
}
