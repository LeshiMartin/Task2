using System.Text;
using HttpApi.Host.DAL;
using HttpApi.Host.Entities;
using HttpApi.Host.Services.AuthServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HttpApi.Host.Extensions;

public static class AuthenticationExtensions
{
  public static WebApplicationBuilder RegisterIdentity ( this WebApplicationBuilder builder )
  {
    builder.Services.AddIdentity<AppUser, IdentityRole> (opt =>
      {

        opt.Lockout.AllowedForNewUsers = true;
        opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes (20);
        opt.Lockout.MaxFailedAccessAttempts = 5;
      })
      .AddEntityFrameworkStores<ApplicationDbContext> ()
      .AddDefaultTokenProviders ();

    var config = builder.Configuration;
    var key = Encoding.ASCII.GetBytes (config[ "secrete" ] ?? "");
    builder.Services.AddAuthentication (options =>
      {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
      })

      // Adding Jwt Bearer  
      .AddJwtBearer (options =>
      {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = config[ "issuer" ],
          ValidAudience = config[ "audience" ],
          IssuerSigningKey = new SymmetricSecurityKey (key)
        };

        options.Events = new JwtBearerEvents
        {
          OnMessageReceived = context =>
          {
            var accessToken = context.Request.Query[ "access_token" ];

            if (string.IsNullOrEmpty(accessToken)) return Task.CompletedTask;
            context.Token = context.Request.Query[ "access_token" ];
            context.Request.Headers.Add ("Authorization", $"Bearer {accessToken}");

            return Task.CompletedTask;
          }
        };
      });
    builder.Services.AddScoped<IAuthService, AuthService> ();
    return builder;
  }
}
