using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using HttpApi.Host.Entities;
using HttpApi.Host.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HttpApi.Host.Services.AuthServices;

internal class AuthService : IAuthService
{
  private readonly UserManager<AppUser> _userManager;
  private readonly IConfiguration _configuration;

  public AuthService ( UserManager<AppUser> userManager ,IConfiguration configuration)
  {
    _userManager = userManager;
    _configuration = configuration;
  }
  public async Task<string> SignInAsync ( LoginModel model, CancellationToken cancellationToken )
  {
    var user = await ValidateLoginRequest(model, cancellationToken);

    var claims = new Claim[] { new (ClaimTypes.NameIdentifier,user.Id) };

    var authSigningKey =
      new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_configuration["secret"] ?? string.Empty));

    var token = new JwtSecurityToken(
      _configuration["issuer"] ?? "",
      _configuration["audience"] ?? "",
      expires: DateTime.Now.AddHours(SystemConstants.TOKEN_EXPIRATION_HOURS),
      claims: claims,
      signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
    return new JwtSecurityTokenHandler ().WriteToken (token);
  }

  private async Task<AppUser> ValidateLoginRequest(LoginModel model, CancellationToken cancellationToken)
  {
    await new LoginModelValidator().ValidateAndThrowAsync(model, cancellationToken);

    var user = await _userManager.FindByNameAsync(model.UserName);
    if (user is null)
      throw new ArgumentException("No user matches  the username");

    if (!await _userManager.CheckPasswordAsync(user, model.Password))
      throw new ArgumentException("The password is wrong");
    return user;
  }

  public async Task<IdentityResult> RegisterAsync ( RegisterModel model, CancellationToken cancellationToken )
  {
    await ValidateRegisterRequest (model, cancellationToken);
    var user = new AppUser (model.UserName);
    return await _userManager.CreateAsync (user, model.Password.Trim ());
  }

  private async Task ValidateRegisterRequest ( RegisterModel model, CancellationToken cancellationToken )
  {
    await new RegisterModelValidator ().ValidateAndThrowAsync (model, cancellationToken);
    var sameUserName = await _userManager.FindByNameAsync (model.UserName);
    if ( sameUserName is not null )
      throw new ArgumentException ("The username is taken");
  }
}
