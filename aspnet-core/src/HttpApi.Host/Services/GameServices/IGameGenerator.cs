using HttpApi.Host.Entities;
using HttpApi.Host.Models.Games;

namespace HttpApi.Host.Services.GameServices;

public interface IGameGenerator
{
  Task<Game> GenerateGame ( CancellationToken cancellationToken );
}
