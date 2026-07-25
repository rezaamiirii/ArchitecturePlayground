namespace ModularMonolith.Modules.Orders.Domain;

internal sealed class OrderItem
{
    private OrderItem()
    {
    }

    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }

    public Guid ProductId { get; private set; }

    public string ProductName { get; private set; } = string.Empty;

    public decimal UnitPrice { get; private set; }

    public int Quantity { get; private set; }

    public decimal LineTotal { get; private set; }

    internal static OrderItem Create(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        return new OrderItem
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ProductName = productName,
            UnitPrice = unitPrice,
            Quantity = quantity,
            LineTotal = unitPrice * quantity,
        };
    }
}
