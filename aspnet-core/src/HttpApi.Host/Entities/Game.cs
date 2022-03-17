namespace HttpApi.Host.Entities;

public class Game
{
  public int Id { get; set; }
  public DateTime CreationTime { get; set; } = DateTime.Now;
  public double CorrectAnswer { get; set; }
  public string AnswerValue { get; set; } = string.Empty;
  public string Condition { get; set; } = string.Empty;
  public bool IsFinished { get; set; }
  public ICollection<UserGame> UserGames { get; set; } = new HashSet<UserGame> ();

}
