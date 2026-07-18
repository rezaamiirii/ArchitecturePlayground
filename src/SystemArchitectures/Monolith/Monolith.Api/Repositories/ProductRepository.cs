using Microsoft.EntityFrameworkCore;
using Monolith.Api.Data;
using Monolith.Api.Models;

namespace Monolith.Api.Repositories;

public sealed class ProductRepository(AppDbContext dbContext)
{
    public AppDbContext DbContext => dbContext;
    public Task<List<Product>> ListAsync() => dbContext.Products.OrderBy(x => x.Id).ToListAsync();
    public Task<Product?> GetByIdAsync(int id) => dbContext.Products.FindAsync(id).AsTask();
    public void Add(Product product) => dbContext.Products.Add(product);
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
