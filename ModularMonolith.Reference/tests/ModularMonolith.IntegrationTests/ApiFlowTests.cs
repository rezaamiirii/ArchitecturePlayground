using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using ModularMonolith.IntegrationTests.Models;
using Xunit;

namespace ModularMonolith.IntegrationTests;

public sealed class ApiFlowTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ApiFlowTests(WebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(_ => { })
            .CreateClient();
    }

    [Fact]
    public async Task Creates_User_Product_And_Order_And_Reduces_Stock()
    {
        var user = await CreateUserAsync("Test", "Buyer", "buyer@example.com");
        var product = await CreateProductAsync("Desk", price: 100m, stock: 3);

        var orderResponse = await _client.PostAsJsonAsync(
            "/api/orders",
            new
            {
                UserId = user.Id,
                Items = new[]
                {
                    new { ProductId = product.Id, Quantity = 2 },
                },
            });

        Assert.Equal(HttpStatusCode.Created, orderResponse.StatusCode);

        var updatedProduct = await _client.GetFromJsonAsync<ProductDto>($"/api/products/{product.Id}");

        Assert.Equal(1, updatedProduct!.AvailableStock);
    }

    [Fact]
    public async Task Rejects_Inactive_User()
    {
        var user = await CreateUserAsync("No", "Buy", "nobuy@example.com");
        await _client.PostAsync($"/api/users/{user.Id}/deactivate", content: null);

        var product = await CreateProductAsync("Chair", price: 80m, stock: 4);

        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            new
            {
                UserId = user.Id,
                Items = new[]
                {
                    new { ProductId = product.Id, Quantity = 1 },
                },
            });

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Rejects_Inactive_Product_And_Insufficient_Stock()
    {
        var user = await CreateUserAsync("Stock", "Tester", "stock@example.com");
        var inactiveProduct = await CreateProductAsync("Old", price: 5m, stock: 9);

        await _client.PostAsync($"/api/products/{inactiveProduct.Id}/deactivate", content: null);

        var inactiveProductResponse = await _client.PostAsJsonAsync(
            "/api/orders",
            new
            {
                UserId = user.Id,
                Items = new[]
                {
                    new { ProductId = inactiveProduct.Id, Quantity = 1 },
                },
            });

        Assert.Equal(HttpStatusCode.Conflict, inactiveProductResponse.StatusCode);

        var lowStockProduct = await CreateProductAsync("Limited", price: 12m, stock: 1);

        var insufficientStockResponse = await _client.PostAsJsonAsync(
            "/api/orders",
            new
            {
                UserId = user.Id,
                Items = new[]
                {
                    new { ProductId = lowStockProduct.Id, Quantity = 2 },
                },
            });

        Assert.Equal(HttpStatusCode.Conflict, insufficientStockResponse.StatusCode);
    }

    [Fact]
    public async Task Historical_Order_Prices_Do_Not_Change()
    {
        var user = await CreateUserAsync("Price", "Tester", "price@example.com");
        var product = await CreateProductAsync("Lamp", price: 20m, stock: 5);

        var createOrderResponse = await _client.PostAsJsonAsync(
            "/api/orders",
            new
            {
                UserId = user.Id,
                Items = new[]
                {
                    new { ProductId = product.Id, Quantity = 1 },
                },
            });

        var createdOrder = await createOrderResponse.Content.ReadFromJsonAsync<CreateOrderDto>();

        await _client.PutAsJsonAsync(
            $"/api/products/{product.Id}/price",
            new { Price = 99m });

        var order = await _client.GetFromJsonAsync<OrderDto>($"/api/orders/{createdOrder!.OrderId}");

        Assert.Equal(20m, order!.Items.Single().UnitPrice);
    }

    private async Task<UserDto> CreateUserAsync(string firstName, string lastName, string email)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/users",
            new
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
            });

        return (await response.Content.ReadFromJsonAsync<UserDto>())!;
    }

    private async Task<ProductDto> CreateProductAsync(string name, decimal price, int stock)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/products",
            new
            {
                Name = name,
                Price = price,
                AvailableStock = stock,
            });

        return (await response.Content.ReadFromJsonAsync<ProductDto>())!;
    }
}
