namespace ModularMonolith.Modules.Orders.Api.Requests;

public sealed record CreateOrderItemRequest(
    Guid ProductId,
    int Quantity);
