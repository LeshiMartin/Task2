using FluentValidation;

namespace HttpApi.Host.Models.Auth;

public class LoginModel
{
  public string UserName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
}


internal class LoginModelValidator : AbstractValidator<LoginModel>
{
  public LoginModelValidator ()
  {
    RuleFor (x => x.UserName).NotEmpty ();
    RuleFor (x => x.Password).NotEmpty ();
  }
}