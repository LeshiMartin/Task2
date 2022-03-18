using AutoMapper;
using HttpApi.Host.Entities;
using HttpApi.Host.Models.Games;

namespace HttpApi.Host.Models;

public class MappingProfile : Profile
{
  public MappingProfile ()
  {
    CreateMap<Game, GameModel> ();
    CreateMap<UserGame, GameModel> ()
      .ForMember (x => x.Condition, x => x.MapFrom (c => c.Game!.Condition))
      .ForMember (x => x.SubmittedAnswer, x => x.MapFrom (c => c.ProposedAnswer))
      .ForMember (x => x.AnswerStatus, x => x.MapFrom (c => GetAnswerStatus (c)));
  }

  private static string GetAnswerStatus ( UserGame c )
  {
    return UserGameStatusHelper.IsCorrect (c.UserGameStatus) ? "OK" :
      c.UserGameStatus == (int) UserGameStatuses.InCorrect ? "FAILED" :
      c.UserGameStatus == (int) UserGameStatuses.Missed ? "MISSED" : "";
  }
}
