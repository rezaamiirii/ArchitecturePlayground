namespace ModularMonolith.Modules.Products.Contracts;

public sealed record ProductOrderInfo(
    Guid Id,
    string Name,
    decimal Price,
    int AvailableStock,
    bool IsActive);
