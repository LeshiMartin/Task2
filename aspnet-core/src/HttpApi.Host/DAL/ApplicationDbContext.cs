using HttpApi.Host.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HttpApi.Host.DAL;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
  public ApplicationDbContext ( DbContextOptions<ApplicationDbContext> options ) : base (options)
  {

  }

  public  DbSet<Game> Games { get; set; }
  public  DbSet<UserGame> UserGames { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.Entity<Game>(b =>
    {
      b.HasMany(x => x.UserGames)
        .WithOne(x => x.Game)
        .HasForeignKey(x => x.GameId);

      b.Property(x => x.AnswerValue).IsRequired().HasMaxLength(20);
      b.Property(x => x.Condition).IsRequired().HasMaxLength(250);
    });

    builder.Entity<UserGame>(b =>
    {
      b.HasOne(x => x.Game)
        .WithMany(x => x.UserGames)
        .HasForeignKey(x => x.GameId);
      b.HasOne(x => x.User)
        .WithMany(x => x.UserGames)
        .HasForeignKey(x => x.UserId);
      b.Property(x => x.ProposedAnswer)
        .IsRequired()
        .HasMaxLength(250);
    });
    base.OnModelCreating(builder);
  }
}
