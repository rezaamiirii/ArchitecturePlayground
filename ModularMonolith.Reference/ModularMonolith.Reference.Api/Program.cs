using ModularMonolith.Modules.Orders;
using ModularMonolith.Modules.Products;
using ModularMonolith.Modules.Users;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddUsersModule(builder.Configuration)
    .AddProductsModule(builder.Configuration)
    .AddOrdersModule(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.Services.SeedUsersAsync();
    await app.Services.SeedProductsAsync();
    await app.Services.EnsureOrdersDatabaseAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();

public partial class Program;
