using HttpApi.Host.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace HttpApi.Host.Services.AuthServices
{
  public interface IAuthService
  {
    Task<string> SignInAsync ( LoginModel model,CancellationToken cancellationToken );
    Task<IdentityResult> RegisterAsync ( RegisterModel model,CancellationToken cancellationToken );
  }
}
