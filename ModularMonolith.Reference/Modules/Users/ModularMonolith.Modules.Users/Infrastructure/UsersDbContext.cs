using Microsoft.EntityFrameworkCore; using ModularMonolith.Modules.Users.Domain;
namespace ModularMonolith.Modules.Users.Infrastructure;
internal sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options)
{ internal DbSet<User> Users => Set<User>(); protected override void OnModelCreating(ModelBuilder b){ var e=b.Entity<User>(); e.ToTable("Users"); e.HasKey(x=>x.Id); e.Property(x=>x.FirstName).HasMaxLength(100); e.Property(x=>x.LastName).HasMaxLength(100); e.Property(x=>x.Email).HasMaxLength(320); e.HasIndex(x=>x.Email).IsUnique(); } }
