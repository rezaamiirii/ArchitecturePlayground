namespace ModularMonolith.IntegrationTests.Models;

internal sealed record OrderDto(
    Guid Id,
    Guid UserId,
    decimal Total,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<OrderItemDto> Items);
