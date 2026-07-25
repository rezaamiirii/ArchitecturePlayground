using Microsoft.EntityFrameworkCore;
using ModularMonolith.Modules.Users.Domain;

namespace ModularMonolith.Modules.Users.Infrastructure;

public sealed class UsersDbContext : DbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {
    }

    internal DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var user = modelBuilder.Entity<User>();

        user.ToTable("Users");
        user.HasKey(entity => entity.Id);
        user.Property(entity => entity.FirstName).HasMaxLength(100);
        user.Property(entity => entity.LastName).HasMaxLength(100);
        user.Property(entity => entity.Email).HasMaxLength(320);
        user.HasIndex(entity => entity.Email).IsUnique();
    }
}
