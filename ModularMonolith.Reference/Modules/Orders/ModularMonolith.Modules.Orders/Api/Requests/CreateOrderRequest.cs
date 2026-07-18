namespace ModularMonolith.Modules.Orders.Api.Requests;

public sealed record CreateOrderRequest(
    Guid UserId,
    IReadOnlyCollection<CreateOrderItemRequest> Items);
