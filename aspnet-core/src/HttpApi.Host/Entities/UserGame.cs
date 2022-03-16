namespace HttpApi.Host.Entities;

public class UserGame
{
  public int Id { get; set; }
  public string UserId { get; set; } = string.Empty;
  public int GameId { get; set; }
  public bool IsFinished { get; set; }
  public string ProposedAnswer { get; set; } = string.Empty;
  public bool IsCorrect { get; set; }
  public AppUser? User { get; set; }
  public Game? Game { get; set; }
}
