namespace ModularMonolith.Modules.Products.Contracts;

public interface IProductsModule
{
    Task<ProductOrderInfo?> GetProductForOrderAsync(
        Guid productId,
        CancellationToken cancellationToken);

    Task ReserveStockAsync(
        Guid productId,
        int quantity,
        CancellationToken cancellationToken);
}
