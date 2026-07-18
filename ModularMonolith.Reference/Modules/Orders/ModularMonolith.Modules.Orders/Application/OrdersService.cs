using Microsoft.EntityFrameworkCore;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Orders.Domain;
using ModularMonolith.Modules.Orders.Infrastructure;
using ModularMonolith.Modules.Products.Contracts;
using ModularMonolith.Modules.Users.Contracts;

namespace ModularMonolith.Modules.Orders.Application;

public sealed class OrdersService
{
    private readonly OrdersDbContext _dbContext;
    private readonly IProductsModule _productsModule;
    private readonly IUsersModule _usersModule;

    public OrdersService(
        OrdersDbContext dbContext,
        IUsersModule usersModule,
        IProductsModule productsModule)
    {
        _dbContext = dbContext;
        _usersModule = usersModule;
        _productsModule = productsModule;
    }

    public async Task<Result<CreateOrderResponse>> CreateAsync(
        Guid userId,
        IReadOnlyCollection<CreateOrderItem> requestItems,
        CancellationToken cancellationToken)
    {
        if (requestItems.Count == 0)
        {
            return Result<CreateOrderResponse>.Failure(
                "orders.empty",
                "Order must contain at least one item.",
                statusCode: 400);
        }

        if (requestItems.Any(item => item.Quantity <= 0))
        {
            return Result<CreateOrderResponse>.Failure(
                "orders.invalid_quantity",
                "Quantity must be positive.",
                statusCode: 400);
        }

        var user = await _usersModule.GetUserForOrderAsync(userId, cancellationToken);

        if (user is null)
        {
            return Result<CreateOrderResponse>.Failure(
                "orders.user_missing",
                "User was not found.",
                statusCode: 404);
        }

        if (!user.IsActive)
        {
            return Result<CreateOrderResponse>.Failure(
                "orders.user_inactive",
                "User is inactive.",
                statusCode: 409);
        }

        var snapshots = new List<(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)>();

        foreach (var item in requestItems)
        {
            var product = await _productsModule.GetProductForOrderAsync(item.ProductId, cancellationToken);

            if (product is null)
            {
                return Result<CreateOrderResponse>.Failure(
                    "orders.product_missing",
                    "Product was not found.",
                    statusCode: 404);
            }

            if (!product.IsActive)
            {
                return Result<CreateOrderResponse>.Failure(
                    "orders.product_inactive",
                    $"Product {product.Name} is inactive.",
                    statusCode: 409);
            }

            if (product.AvailableStock < item.Quantity)
            {
                return Result<CreateOrderResponse>.Failure(
                    "orders.insufficient_stock",
                    $"Product {product.Name} does not have enough stock.",
                    statusCode: 409);
            }

            snapshots.Add((product.Id, product.Name, product.Price, item.Quantity));
        }

        var order = Order.Create(userId, snapshots);

        foreach (var item in requestItems)
        {
            await _productsModule.ReserveStockAsync(
                item.ProductId,
                item.Quantity,
                cancellationToken);
        }

        _dbContext.Orders.Add(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateOrderResponse>.Success(
            new CreateOrderResponse(order.Id),
            statusCode: 201);
    }

    public async Task<Result<OrderResponse>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .Include(entity => entity.Items)
            .SingleOrDefaultAsync(entity => entity.Id == id, cancellationToken);

        if (order is null)
        {
            return Result<OrderResponse>.Failure(
                "orders.not_found",
                "Order was not found.",
                statusCode: 404);
        }

        return Result<OrderResponse>.Success(Map(order));
    }

    public async Task<IReadOnlyCollection<OrderResponse>> ListAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
            .Include(order => order.Items)
            .OrderByDescending(order => order.CreatedAtUtc)
            .Select(order => Map(order))
            .ToListAsync(cancellationToken);
    }

    private static OrderResponse Map(Order order)
    {
        return new OrderResponse(
            order.Id,
            order.UserId,
            order.Total,
            order.CreatedAtUtc,
            order.Items
                .Select(item => new OrderItemResponse(
                    item.ProductId,
                    item.ProductName,
                    item.UnitPrice,
                    item.Quantity,
                    item.LineTotal))
                .ToList());
    }
}
