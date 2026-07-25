using DistributedMonolith.Users.Api;
using DistributedMonolith.Users.Application;
using DistributedMonolith.Users.Infrastructure;
using Microsoft.EntityFrameworkCore;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<UsersDbContext>(o=>o.UseSqlite(builder.Configuration.GetConnectionString("Database")??"Data Source=Users.db"));
builder.Services.AddScoped<UserService>(); builder.Services.AddEndpointsApiExplorer(); builder.Services.AddSwaggerGen();
var app=builder.Build(); using(var scope=app.Services.CreateScope()){var db=scope.ServiceProvider.GetRequiredService<UsersDbContext>();db.Database.EnsureCreated();}
app.UseSwagger();app.UseSwaggerUI();app.MapUserEndpoints();app.MapGet("/health",()=>Results.Ok());app.Run();
