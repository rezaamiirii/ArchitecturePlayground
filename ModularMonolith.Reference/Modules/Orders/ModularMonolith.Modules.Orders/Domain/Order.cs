namespace ModularMonolith.Modules.Orders.Domain;

internal sealed class Order
{
    private readonly List<OrderItem> _items = [];

    private Order()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public decimal Total { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items;

    public static Order Create(
        Guid userId,
        IEnumerable<(Guid ProductId, string ProductName, decimal UnitPrice, int Quantity)> items)
    {
        var orderItems = items.ToList();

        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User is required.");
        }

        if (orderItems.Count == 0)
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CreatedAtUtc = DateTime.UtcNow,
        };

        foreach (var item in orderItems)
        {
            order.AddItem(item.ProductId, item.ProductName, item.UnitPrice, item.Quantity);
        }

        order.Total = order._items.Sum(item => item.LineTotal);

        return order;
    }

    private void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        if (productId == Guid.Empty)
        {
            throw new ArgumentException("Product is required.");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (unitPrice <= 0)
        {
            throw new ArgumentException("Unit price must be positive.");
        }

        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.");
        }

        _items.Add(OrderItem.Create(productId, productName, unitPrice, quantity));
    }
}
