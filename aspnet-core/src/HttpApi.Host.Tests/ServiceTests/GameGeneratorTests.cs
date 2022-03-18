using FluentAssertions;
using HttpApi.Host.DAL;
using HttpApi.Host.Services.GameServices;
using NSubstitute;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace HttpApi.Host.Tests.ServiceTests;
public class GameGeneratorTests
{
  private readonly GameGenerator _gameGenerator;

  public GameGeneratorTests ()
  {
    var repo = Substitute.For<IGameRepo> ();

    _gameGenerator = new GameGenerator (repo);
  }
  [Theory]
  [InlineData (3)]
  public void GenerateCondition_Should_Randomize_The_Output ( int reccurence )
  {
    while ( reccurence > 0 )
    {
      var responses = new List<string> ();
      while ( responses.Count < 1000 )
        responses.Add (_gameGenerator.GenerateOperationSign ());
      var grouped = responses
        .GroupBy (x => x)
        .Select (x => new { condition = x.Key, percent = x.Count () / 10 })
        .ToArray ();

      var max = grouped.Max (x => x.percent);
      var min = grouped.Min (x => x.percent);
      var diff = max - min;
      diff.Should ().BeLessThanOrEqualTo (10);
      reccurence--;
    }

  }

  [Theory]
  [InlineData (3, 5, "+", 8)]
  [InlineData (3, 5, "*", 15)]
  [InlineData (12, 5, "/", 2.4)]
  [InlineData (3, 5, "-", -2)]
  public void GenerateSolutionValue_Should_ProduceValue_That_Match_Expected ( int x, int y, string op, double expected )
  {
    var res = GameGenerator.GenerateSolutionValue (x, y, op);
    res.Should ().Be (expected);
  }
}
