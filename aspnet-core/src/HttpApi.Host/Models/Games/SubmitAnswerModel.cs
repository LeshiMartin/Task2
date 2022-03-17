namespace HttpApi.Host.Models.Games;

public record SubmitAnswerModel ( int GameId, string Answer, string UserId );