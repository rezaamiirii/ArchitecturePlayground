using Microsoft.AspNetCore.Diagnostics;
using Monolith.Api.Configuration;
using Monolith.Api.Data;
using Monolith.Api.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMonolith(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
{
    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    context.Response.StatusCode = exception switch
    {
        NotFoundException => StatusCodes.Status404NotFound,
        ValidationException => StatusCodes.Status400BadRequest,
        ConflictException => StatusCodes.Status409Conflict,
        _ => StatusCodes.Status500InternalServerError
    };
    await context.Response.WriteAsJsonAsync(new { error = exception?.Message ?? "An unexpected error occurred." });
}));

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    using var scope = app.Services.CreateScope();
    await DatabaseInitializer.InitializeAsync(scope.ServiceProvider.GetRequiredService<AppDbContext>());
}

app.MapControllers();
app.Run();
