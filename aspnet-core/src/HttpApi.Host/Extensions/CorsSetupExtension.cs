namespace HttpApi.Host.Extensions;

public static class CorsSetupExtension
{
  private const string POLICY_NAME = "ANGULAR_ACCESS";
  public static WebApplicationBuilder RegisterCors ( this WebApplicationBuilder builder )
  {

    var endPoint = builder.Configuration[ "Cors_Endpoint" ] ?? "";
    builder.Services.AddCors (b => b.AddPolicy (POLICY_NAME, x => x.WithOrigins (endPoint)
        .AllowAnyHeader ()
        .AllowAnyMethod ()
        .AllowCredentials ()));
    return builder;
  }

  public static void UseWebCors ( this WebApplication app )
  {
    app.UseCors (POLICY_NAME);
  }

}
