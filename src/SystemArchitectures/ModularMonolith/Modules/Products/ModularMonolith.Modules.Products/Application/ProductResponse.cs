namespace ModularMonolith.Modules.Products.Application;

public sealed record ProductResponse(
    Guid Id,
    string Name,
    decimal Price,
    int AvailableStock,
    bool IsActive,
    DateTime CreatedAtUtc);
