using Domain;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Game> Games { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasOne(u => u.AsOwner)
            .WithOne(g => g.Player1);

        modelBuilder.Entity<User>()
            .HasOne(u => u.AsPlayer)
            .WithOne(g => g.Player2);

        modelBuilder.Entity<User>()
            .HasOne(u => u.AsWatcher)
            .WithMany(g => g.Others);

        modelBuilder.Entity<User>()
            .Ignore(x => x.ActiveGame)
            .Ignore(x => x.HasJoinedGame);
    }
}
