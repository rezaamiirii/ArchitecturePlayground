namespace ModularMonolith.Modules.Orders.Application;

public sealed record OrderResponse(
    Guid Id,
    Guid UserId,
    decimal Total,
    DateTime CreatedAtUtc,
    IReadOnlyCollection<OrderItemResponse> Items);
