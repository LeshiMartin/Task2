namespace HttpApi.Host.Exceptions;

public class GameFinishedException : Exception
{
  public GameFinishedException () : base ("This game is finished, you cannot submit answer")
  { }
}
