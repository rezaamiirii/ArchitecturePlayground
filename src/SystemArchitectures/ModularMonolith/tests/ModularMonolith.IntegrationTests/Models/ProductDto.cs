namespace ModularMonolith.IntegrationTests.Models;

internal sealed record ProductDto(
    Guid Id,
    string Name,
    decimal Price,
    int AvailableStock,
    bool IsActive,
    DateTime CreatedAtUtc);
