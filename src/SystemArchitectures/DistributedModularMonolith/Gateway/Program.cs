using Yarp.ReverseProxy.Configuration;
var builder=WebApplication.CreateBuilder(args);
builder.Services.AddReverseProxy().LoadFromMemory(
 [new("users-route","users",new(){Path="/api/users/{**catch-all}"}),new("products-route","products",new(){Path="/api/products/{**catch-all}"}),new("orders-route","orders",new(){Path="/api/orders/{**catch-all}"})],
 [new("users",[new("users",new(){Address=builder.Configuration["Services:Users"]??"http://localhost:5101/"})]),new("products",[new("products",new(){Address=builder.Configuration["Services:Products"]??"http://localhost:5102/"})]),new("orders",[new("orders",new(){Address=builder.Configuration["Services:Orders"]??"http://localhost:5103/"})])]);
var app=builder.Build();app.MapGet("/",()=>Results.Ok(new{name="Distributed Modular Monolith Gateway",release="1.0.0"}));app.MapReverseProxy();app.Run();
