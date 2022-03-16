using FluentValidation;

namespace HttpApi.Host.Models.Auth;

public class RegisterModel
{
  public string UserName { get; set; } = string.Empty;
  public string Password { get; set; } = string.Empty;
  public string ConfirmPassword { get; set; } = string.Empty;
}

internal class RegisterModelValidator : AbstractValidator<RegisterModel>
{
  public RegisterModelValidator ()
  {
    RuleFor (x => x.UserName).NotEmpty ();
    RuleFor (x => x.Password).NotEmpty ();
    RuleFor (x => x.ConfirmPassword).NotEmpty ().Must (( x, y ) => x.Password == y);  
  }
}