using HttpApi.Host.DAL;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HttpApi.Host.Extensions;

public static class DbContextExtensions
{
  public static WebApplicationBuilder RegisterDbContext(this WebApplicationBuilder builder)
  {
    var config = builder.Configuration;
    var server = config["DbServer"] ?? "localhost";
    var user = config["DbUser"] ?? "sa"; // Warning do not use the SA account
    var password = config["Password"] ?? "P@ssw0rd1!";
    var database = config["Database"] ?? "GameTask";

    var cString = $"Server={server};Database={database};User Id={user};Password={password}";

    // Add Db context as a service to our application
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
      options.UseSqlServer(cString));
    return builder;
  }

  public static void ExecuteMigrations(this WebApplication app)
  {
    using var serviceScope = app.Services.CreateScope();
    serviceScope
      .ServiceProvider
      .GetRequiredService<ApplicationDbContext>()
      .Database
      .Migrate();
  }
}
