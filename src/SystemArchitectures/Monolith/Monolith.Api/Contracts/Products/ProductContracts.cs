namespace Monolith.Api.Contracts.Products;

public sealed record CreateProductRequest(string Name, string Description, decimal Price, int AvailableStock);
public sealed record UpdateProductPriceRequest(decimal Price);
public sealed record UpdateProductStockRequest(int AvailableStock);
public sealed record ProductResponse(int Id, string Name, string Description, decimal Price, int AvailableStock, bool IsActive, DateTime CreatedAtUtc);
