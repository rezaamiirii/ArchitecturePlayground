using Microsoft.EntityFrameworkCore;
using ModularMonolith.BuildingBlocks.Errors;
using ModularMonolith.Modules.Products.Contracts;
using ModularMonolith.Modules.Products.Domain;
using ModularMonolith.Modules.Products.Infrastructure;

namespace ModularMonolith.Modules.Products.Application;

public sealed class ProductsService : IProductsModule
{
    private readonly ProductsDbContext _dbContext;

    public ProductsService(ProductsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ProductResponse>> CreateAsync(
        string name,
        decimal price,
        int stock,
        CancellationToken cancellationToken)
    {
        try
        {
            var product = Product.Create(name, price, stock);

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<ProductResponse>.Success(
                Map(product),
                statusCode: 201);
        }
        catch (ArgumentException exception)
        {
            return Result<ProductResponse>.Failure(
                "products.invalid",
                exception.Message,
                statusCode: 400);
        }
    }

    public async Task<Result<ProductResponse>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(
                "products.not_found",
                "Product was not found.",
                statusCode: 404);
        }

        return Result<ProductResponse>.Success(Map(product));
    }

    public async Task<Result<ProductResponse>> SetActiveAsync(
        Guid id,
        bool active,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(
                "products.not_found",
                "Product was not found.",
                statusCode: 404);
        }

        if (active)
        {
            product.Activate();
        }
        else
        {
            product.Deactivate();
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<ProductResponse>.Success(Map(product));
    }

    public async Task<Result<ProductResponse>> AddStockAsync(
        Guid id,
        int quantity,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(
                "products.not_found",
                "Product was not found.",
                statusCode: 404);
        }

        try
        {
            product.IncreaseStock(quantity);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<ProductResponse>.Success(Map(product));
        }
        catch (ArgumentException exception)
        {
            return Result<ProductResponse>.Failure(
                "products.invalid",
                exception.Message,
                statusCode: 400);
        }
    }

    public async Task<Result<ProductResponse>> ChangePriceAsync(
        Guid id,
        decimal price,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([id], cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure(
                "products.not_found",
                "Product was not found.",
                statusCode: 404);
        }

        try
        {
            product.ChangePrice(price);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Result<ProductResponse>.Success(Map(product));
        }
        catch (ArgumentException exception)
        {
            return Result<ProductResponse>.Failure(
                "products.invalid",
                exception.Message,
                statusCode: 400);
        }
    }

    public async Task<ProductOrderInfo?> GetProductForOrderAsync(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Products
            .Where(product => product.Id == productId)
            .Select(product => new ProductOrderInfo(
                product.Id,
                product.Name,
                product.Price,
                product.AvailableStock,
                product.IsActive))
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task ReserveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken)
    {
        var product = await _dbContext.Products.FindAsync([productId], cancellationToken)
            ?? throw new InvalidOperationException("Product was not found.");

        product.DecreaseStock(quantity);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private static ProductResponse Map(Product product)
    {
        return new ProductResponse(
            product.Id,
            product.Name,
            product.Price,
            product.AvailableStock,
            product.IsActive,
            product.CreatedAtUtc);
    }
}
