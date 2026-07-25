namespace DistributedMonolith.SharedContracts;

public sealed record CreateUserRequest(string Name, string Email);
public sealed record UpdateUserRequest(string Name, string Email);
public sealed record UserResponse(Guid Id, string Name, string Email);

public sealed record CreateProductRequest(string Name, decimal Price, int Stock);
public sealed record UpdateStockRequest(int Quantity);
public sealed record ProductResponse(Guid Id, string Name, decimal Price, int Stock);
public sealed record StockReservationRequest(int Quantity);
public sealed record StockReservationResponse(bool Reserved, string? Reason = null);

public sealed record CreateOrderRequest(Guid UserId, Guid ProductId, int Quantity);
public sealed record OrderResponse(Guid Id, Guid UserId, Guid ProductId, int Quantity, decimal UnitPrice, string Status, DateTimeOffset CreatedAt);
