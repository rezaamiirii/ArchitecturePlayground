using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Monolith.Api.Contracts.Orders;
using Monolith.Api.Data;
using Monolith.Api.Exceptions;
using Monolith.Api.Models;
using Monolith.Api.Repositories;
using Monolith.Api.Services;

namespace Monolith.Api.Tests;

public sealed class MonolithServiceTests : IDisposable
{
    private readonly SqliteConnection _connection = new("Data Source=:memory:");
    private readonly AppDbContext _db;
    private readonly UserRepository _users;
    private readonly ProductRepository _products;
    private readonly OrderRepository _orders;
    private readonly UserService _userService;
    private readonly ProductService _productService;
    private readonly OrderService _orderService;

    public MonolithServiceTests()
    {
        _connection.Open();
        _db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);
        _db.Database.EnsureCreated();
        _users = new UserRepository(_db);
        _products = new ProductRepository(_db);
        _orders = new OrderRepository(_db);
        _userService = new UserService(_users);
        _productService = new ProductService(_products);
        _orderService = new OrderService(_users, _products, _orders);
    }

    [Fact] public async Task CreateUser_Creates_active_user() { var u = await _userService.CreateAsync("Ada", "Lovelace", "ADA@example.com"); Assert.True(u.IsActive); Assert.Equal("ada@example.com", u.Email); }
    [Fact] public async Task CreateUser_Rejects_duplicate_email() { await _userService.CreateAsync("A", "B", "a@b.com"); await Assert.ThrowsAsync<ConflictException>(() => _userService.CreateAsync("C", "D", "a@b.com")); }
    [Fact] public async Task CreateProduct_Creates_product() { var p = await _productService.CreateAsync("Book", "Architecture", 20m, 3); Assert.Equal(20m, p.Price); Assert.True(p.IsActive); }
    [Fact] public async Task CreateProduct_Rejects_negative_price() => await Assert.ThrowsAsync<ValidationException>(() => _productService.CreateAsync("Bad", "", -1m, 1));

    [Fact]
    public async Task CreateOrder_For_active_user_creates_order()
    {
        var (user, product) = await SeedActiveUserAndProductAsync();
        var order = await _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 2)]));
        Assert.Equal(OrderStatus.Confirmed, order.Status);
    }

    [Fact]
    public async Task CreateOrder_Rejects_inactive_user()
    {
        var user = await _userService.CreateAsync("Inactive", "User", "inactive@example.com");
        await _userService.DeactivateAsync(user.Id);
        var product = await _productService.CreateAsync("Pen", "", 2m, 10);
        await Assert.ThrowsAsync<ConflictException>(() => _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 1)])));
    }

    [Fact]
    public async Task CreateOrder_Rejects_inactive_product()
    {
        var user = await _userService.CreateAsync("Active", "User", "active@example.com");
        var product = await _productService.CreateAsync("Old", "", 2m, 10);
        await _productService.DeactivateAsync(product.Id);
        await Assert.ThrowsAsync<ConflictException>(() => _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 1)])));
    }

    [Fact]
    public async Task CreateOrder_Rejects_insufficient_stock()
    {
        var (user, product) = await SeedActiveUserAndProductAsync(stock: 1);
        await Assert.ThrowsAsync<ConflictException>(() => _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 2)])));
    }

    [Fact]
    public async Task CreateOrder_Reduces_stock_after_success()
    {
        var (user, product) = await SeedActiveUserAndProductAsync(stock: 5);
        await _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 2)]));
        Assert.Equal(3, (await _productService.GetByIdAsync(product.Id)).AvailableStock);
    }

    [Fact]
    public async Task CreateOrder_Stores_product_snapshots()
    {
        var (user, product) = await SeedActiveUserAndProductAsync(price: 9m);
        var order = await _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(product.Id, 1)]));
        await _productService.UpdatePriceAsync(product.Id, 99m);
        Assert.Equal(product.Name, order.Items[0].ProductName);
        Assert.Equal(9m, order.Items[0].UnitPrice);
    }

    [Fact]
    public async Task CreateOrder_Calculates_totals()
    {
        var user = await _userService.CreateAsync("Total", "User", "total@example.com");
        var p1 = await _productService.CreateAsync("A", "", 5m, 10);
        var p2 = await _productService.CreateAsync("B", "", 3m, 10);
        var order = await _orderService.CreateAsync(new CreateOrderRequest(user.Id, [new CreateOrderItemRequest(p1.Id, 2), new CreateOrderItemRequest(p2.Id, 3)]));
        Assert.Equal(19m, order.TotalAmount);
    }

    [Fact] public async Task CancelOrder_Cancels_order() { var (u, p) = await SeedActiveUserAndProductAsync(); var o = await _orderService.CreateAsync(new CreateOrderRequest(u.Id, [new CreateOrderItemRequest(p.Id, 1)])); Assert.Equal(OrderStatus.Cancelled, (await _orderService.CancelAsync(o.Id)).Status); }
    [Fact] public void Repositories_Use_same_AppDbContext_type() { Assert.Same(_db, _users.DbContext); Assert.Same(_db, _products.DbContext); Assert.Same(_db, _orders.DbContext); }

    [Fact]
    public void Monolith_sample_has_only_one_executable_application_project()
    {
        var root = FindMonolithRoot();
        var productionProjects = Directory.GetFiles(root, "*.csproj", SearchOption.AllDirectories).Where(p => !p.Contains($"{Path.DirectorySeparatorChar}tests{Path.DirectorySeparatorChar}")).ToList();
        Assert.Single(productionProjects);
        Assert.EndsWith("Monolith.Api.csproj", productionProjects[0]);
    }

    private async Task<(User User, Product Product)> SeedActiveUserAndProductAsync(decimal price = 10m, int stock = 10)
        => (await _userService.CreateAsync(Guid.NewGuid().ToString("N"), "User", $"{Guid.NewGuid():N}@example.com"), await _productService.CreateAsync(Guid.NewGuid().ToString("N"), "", price, stock));
    private static string FindMonolithRoot() { var dir = new DirectoryInfo(AppContext.BaseDirectory); while (dir is not null && !Directory.Exists(Path.Combine(dir.FullName, "src", "SystemArchitectures", "Monolith"))) dir = dir.Parent; return Path.Combine(dir!.FullName, "src", "SystemArchitectures", "Monolith"); }
    public void Dispose() { _db.Dispose(); _connection.Dispose(); }
}
