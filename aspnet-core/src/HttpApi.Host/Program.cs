using FluentValidation;
using FluentValidation.AspNetCore;
using HttpApi.Host;
using HttpApi.Host.Extensions;
using HttpApi.Host.Hubs;
using HttpApi.Host.Models.Auth;
using HttpApi.Host.Services.AuthServices;
using MediatR;

CancellationToken cancellationToken () => new CancellationTokenSource (TimeSpan.FromMinutes (1)).Token;
var builder = WebApplication.CreateBuilder (args);
builder.Services.AddSignalR ();
builder.RegisterDbContext ();
builder.RegisterIdentity ();
builder.RegisterRedis ();
builder.RegisterCors ();
builder.RegisterNotificationImplementations ();
builder.RegisterAutoMapper ();
builder.RegisterGameServices ();
builder.Services.AddFluentValidation ();
builder.Services.AddMediatR (typeof (Program));
builder.WebHost.UseUrls ("http://*80");

var app = builder.Build ();
app.ExecuteMigrations ();
app.UseWebCors ();
app.UseAuthentication ();
app.UseRouting ();
app.UseAuthorization ();
app.MapGet ("/", () => Results.Ok("Hello"));
app.MapPost ("/login", async ( IAuthService authService, ILogger<Program> logger, LoginModel loginModel ) =>
 {
   try
   {
     var token = await authService.SignInAsync (loginModel, cancellationToken ());
     return Results.Ok (new
     {
       expiresAt = DateTime.Now.AddHours (SystemConstants.TOKEN_EXPIRATION_HOURS),
       token
     });
   }
   catch ( ValidationException exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.BadRequest (exc.Message);
   }
   catch ( ArgumentException exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.BadRequest (exc.Message);
   }
   catch ( Exception exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.StatusCode (500);
   }
 });

app.MapPost ("/Register", async ( IAuthService authService, ILogger<Program> logger, RegisterModel registerModel ) =>
 {
   try
   {
     var result = await authService
       .RegisterAsync (registerModel, cancellationToken ());
     var response = result.Succeeded ?
       Results.Ok () :
       Results.BadRequest (result.Errors);
     return response;
   }
   catch ( ValidationException exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.BadRequest (exc.Message);
   }
   catch ( ArgumentException exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.BadRequest (exc.Message);
   }
   catch ( Exception exc )
   {
     logger.LogError (exc, "{message}", exc.Message);
     return Results.StatusCode (500);
   }
 });

app.UseEndpoints (e =>
 {
   e.MapHub<MainHub> ("/hubs/MainHub");
 });
app.Run ();
