using HttpApi.Host.Models.Games;

namespace HttpApi.Host.Services.GameServices;

public interface ISubmitAnswer
{
  Task<bool> SubmitAsync ( SubmitAnswerModel model, CancellationToken cancellationToken );
}