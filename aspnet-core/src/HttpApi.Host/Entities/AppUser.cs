using Microsoft.AspNetCore.Identity;

namespace HttpApi.Host.Entities;

public class AppUser : IdentityUser
{
  private AppUser()
  {

  }

  public AppUser(string userName) : this()
  {
    UserName = userName;
    SecurityStamp = Guid.NewGuid().ToString()[..6];
  }
  public ICollection<UserGame> UserGames { get; set; } = new HashSet<UserGame>();
}
