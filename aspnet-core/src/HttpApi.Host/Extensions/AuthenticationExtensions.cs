using HttpApi.Host.DAL;
using HttpApi.Host.Entities;
using HttpApi.Host.Services.AuthServices;
using Microsoft.AspNetCore.Identity;

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
    builder.Services.AddScoped<IAuthService, AuthService> ();
    return builder;
  }
}
