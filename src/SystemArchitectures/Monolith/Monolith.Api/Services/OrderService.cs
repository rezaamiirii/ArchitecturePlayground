using Monolith.Api.Contracts.Orders;
using Monolith.Api.Exceptions;
using Monolith.Api.Models;
using Monolith.Api.Repositories;

namespace Monolith.Api.Services;

public sealed class OrderService(UserRepository users, ProductRepository products, OrderRepository orders)
{
    public Task<List<Order>> ListAsync() => orders.ListAsync();
    public async Task<Order> GetByIdAsync(int id) => await orders.GetByIdAsync(id) ?? throw new NotFoundException($"Order {id} was not found.");

    public async Task<Order> CreateAsync(CreateOrderRequest request)
    {
        if (request.Items.Count == 0) throw new ValidationException("At least one order item is required.");
        await using var transaction = await orders.DbContext.Database.BeginTransactionAsync();
        var user = await users.GetByIdAsync(request.UserId) ?? throw new NotFoundException($"User {request.UserId} was not found.");
        if (!user.IsActive) throw new ConflictException("Inactive users cannot place new orders.");
        var order = new Order { UserId = user.Id, Status = OrderStatus.Confirmed };
        foreach (var item in request.Items)
        {
            if (item.Quantity <= 0) throw new ValidationException("Quantity must be greater than zero.");
            var product = await products.GetByIdAsync(item.ProductId) ?? throw new NotFoundException($"Product {item.ProductId} was not found.");
            if (!product.IsActive) throw new ConflictException("Inactive products cannot be ordered.");
            if (product.AvailableStock < item.Quantity) throw new ConflictException("Sufficient stock is not available.");
            product.AvailableStock -= item.Quantity;
            order.Items.Add(new OrderItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                UnitPrice = product.Price,
                Quantity = item.Quantity,
                LineTotal = product.Price * item.Quantity
            });
        }
        order.TotalAmount = order.Items.Sum(x => x.LineTotal);
        orders.Add(order);
        await orders.SaveChangesAsync();
        await transaction.CommitAsync();
        return order;
    }

    public async Task<Order> CancelAsync(int id)
    {
        var order = await GetByIdAsync(id);
        order.Status = OrderStatus.Cancelled;
        await orders.SaveChangesAsync();
        return order;
    }
}
