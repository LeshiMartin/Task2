namespace HttpApi.Host.Entities;

public class UserGame
{
  public int Id { get; set; }
  public string UserId { get; set; } = string.Empty;
  public int GameId { get; set; }
  public string ProposedAnswer { get; set; } = string.Empty;
  public int UserGameStatus { get; set; } = (int) UserGameStatuses.NotAnswered;
  public AppUser? User { get; set; }
  public Game? Game { get; set; }
}


public enum UserGameStatuses
{
  NotAnswered = 0,
  Correct = 10,
  InCorrect = 20,
  Missed = 30,
}