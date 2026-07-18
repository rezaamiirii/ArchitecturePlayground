using Microsoft.EntityFrameworkCore;
using Monolith.Api.Data;
using Monolith.Api.Models;

namespace Monolith.Api.Repositories;

public sealed class OrderRepository(AppDbContext dbContext)
{
    public AppDbContext DbContext => dbContext;
    public Task<List<Order>> ListAsync() => dbContext.Orders.Include(x => x.Items).OrderBy(x => x.Id).ToListAsync();
    public Task<Order?> GetByIdAsync(int id) => dbContext.Orders.Include(x => x.Items).FirstOrDefaultAsync(x => x.Id == id);
    public void Add(Order order) => dbContext.Orders.Add(order);
    public Task SaveChangesAsync() => dbContext.SaveChangesAsync();
}
