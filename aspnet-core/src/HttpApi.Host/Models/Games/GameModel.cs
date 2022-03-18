namespace HttpApi.Host.Models.Games;

public class GameModel
{
  public int Id { get; set; }
  public int GameId { get; set; }
  public string Condition { get; set; } = string.Empty;
  public string SubmittedAnswer { get; set; } = string.Empty;
  public string AnswerStatus { get; set; } = string.Empty;
}
