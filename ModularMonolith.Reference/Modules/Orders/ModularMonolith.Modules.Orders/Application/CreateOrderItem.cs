namespace ModularMonolith.Modules.Orders.Application;

public sealed record CreateOrderItem(
    Guid ProductId,
    int Quantity);
