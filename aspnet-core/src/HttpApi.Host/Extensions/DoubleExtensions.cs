namespace HttpApi.Host.Extensions;

public static class DoubleExtensions
{
  private const double TOLERANCE = 0.000000001;
  public static bool IsEqualTo(this double x, double y) => Math.Abs(x - y) < TOLERANCE;
}
