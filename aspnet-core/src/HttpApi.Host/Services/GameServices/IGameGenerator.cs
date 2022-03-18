using HttpApi.Host.Entities;

namespace HttpApi.Host.Services.GameServices;

public interface IGameGenerator
{
  Task<Game> GenerateGame ( CancellationToken cancellationToken );
}
