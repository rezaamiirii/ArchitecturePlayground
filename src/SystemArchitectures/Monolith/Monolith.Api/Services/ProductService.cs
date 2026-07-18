using Monolith.Api.Exceptions;
using Monolith.Api.Models;
using Monolith.Api.Repositories;

namespace Monolith.Api.Services;

public sealed class ProductService(ProductRepository products)
{
    public Task<List<Product>> ListAsync() => products.ListAsync();
    public async Task<Product> GetByIdAsync(int id) => await products.GetByIdAsync(id) ?? throw new NotFoundException($"Product {id} was not found.");
    public async Task<Product> CreateAsync(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ValidationException("Name is required.");
        if (price < 0) throw new ValidationException("Price cannot be negative.");
        if (stock < 0) throw new ValidationException("Available stock cannot be negative.");
        var product = new Product { Name = name.Trim(), Description = description.Trim(), Price = price, AvailableStock = stock };
        products.Add(product); await products.SaveChangesAsync(); return product;
    }
    public async Task<Product> UpdatePriceAsync(int id, decimal price)
    {
        if (price < 0) throw new ValidationException("Price cannot be negative.");
        var product = await GetByIdAsync(id); product.Price = price; await products.SaveChangesAsync(); return product;
    }
    public async Task<Product> UpdateStockAsync(int id, int stock)
    {
        if (stock < 0) throw new ValidationException("Available stock cannot be negative.");
        var product = await GetByIdAsync(id); product.AvailableStock = stock; await products.SaveChangesAsync(); return product;
    }
    public async Task<Product> DeactivateAsync(int id)
    {
        var product = await GetByIdAsync(id); product.IsActive = false; await products.SaveChangesAsync(); return product;
    }
}
