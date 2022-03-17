using HttpApi.Host.Entities;
using HttpApi.Host.Extensions;

namespace HttpApi.Host.Models.Games;

public static class UserGameStatusHelper
{
  private static readonly IDictionary<bool, UserGameStatuses> _options =
    new Dictionary<bool, UserGameStatuses> ()
  {
    { true, UserGameStatuses.Correct },
    { false, UserGameStatuses.InCorrect }
  };
  public static UserGameStatuses GetStatus ( string answer, Game game )
  => double.TryParse (answer, out var valAnswer) ?
      _options[ valAnswer.IsEqualTo (game.CorrectAnswer) ] :
      _options[ answer == game.AnswerValue ];

  public static bool IsCorrect ( int val )
    => (UserGameStatuses) val == UserGameStatuses.Correct;
}
