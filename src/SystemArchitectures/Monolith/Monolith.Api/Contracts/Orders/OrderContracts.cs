using Monolith.Api.Models;

namespace Monolith.Api.Contracts.Orders;

public sealed record CreateOrderRequest(int UserId, IReadOnlyList<CreateOrderItemRequest> Items);
public sealed record CreateOrderItemRequest(int ProductId, int Quantity);
public sealed record OrderResponse(int Id, int UserId, DateTime CreatedAtUtc, OrderStatus Status, decimal TotalAmount, IReadOnlyList<OrderItemResponse> Items);
public sealed record OrderItemResponse(int Id, int ProductId, string ProductName, decimal UnitPrice, int Quantity, decimal LineTotal);
