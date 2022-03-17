using HttpApi.Host.DAL;
using Microsoft.EntityFrameworkCore;

namespace HttpApi.Host.Extensions;

public static class DbContextExtensions
{
  public static WebApplicationBuilder RegisterDbContext ( this WebApplicationBuilder builder )
  {
    var config = builder.Configuration;
    var cString = config[ "Connection_String" ] ?? "localhost";
    builder.Services.AddDbContextFactory<ApplicationDbContext> (options =>
       options.UseSqlServer (cString));
    builder.Services.AddScoped<IGameRepo, GameRepo> ();
    return builder;
  }

  public static void ExecuteMigrations ( this WebApplication app )
  {
    using var serviceScope = app.Services.CreateScope ();
    serviceScope
      .ServiceProvider
      .GetRequiredService<IDbContextFactory<ApplicationDbContext>> ()
      .CreateDbContext ()
      .Database
      .Migrate ();
  }
}
