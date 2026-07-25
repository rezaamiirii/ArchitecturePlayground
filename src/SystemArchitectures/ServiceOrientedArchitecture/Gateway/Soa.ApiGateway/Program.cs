var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "SOA API Gateway is running.");

app.Run();
