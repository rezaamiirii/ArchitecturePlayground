namespace Monolith.Api.Models;

public enum OrderStatus
{
    Pending = 0,
    Confirmed = 1,
    Cancelled = 2
}

public sealed class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User? User { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    public OrderStatus Status { get; set; } = OrderStatus.Confirmed;
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = [];
}
