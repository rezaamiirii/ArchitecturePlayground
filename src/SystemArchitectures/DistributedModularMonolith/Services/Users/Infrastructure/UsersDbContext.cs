using DistributedMonolith.Users.Domain;
using Microsoft.EntityFrameworkCore;
namespace DistributedMonolith.Users.Infrastructure;
public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options) { public DbSet<User> Users => Set<User>(); protected override void OnModelCreating(ModelBuilder b) { b.Entity<User>().HasKey(x => x.Id); b.Entity<User>().HasIndex(x => x.Email).IsUnique(); } }
