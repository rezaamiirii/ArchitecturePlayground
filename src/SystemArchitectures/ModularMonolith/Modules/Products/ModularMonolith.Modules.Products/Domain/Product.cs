namespace ModularMonolith.Modules.Products.Domain;

internal sealed class Product
{
    private Product()
    {
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public decimal Price { get; private set; }

    public int AvailableStock { get; private set; }

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public static Product Create(string name, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Product name is required.");
        }

        if (price <= 0)
        {
            throw new ArgumentException("Price must be positive.");
        }

        if (stock < 0)
        {
            throw new ArgumentException("Stock cannot be negative.");
        }

        return new Product
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            Price = price,
            AvailableStock = stock,
            IsActive = true,
            CreatedAtUtc = DateTime.UtcNow,
        };
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ChangePrice(decimal price)
    {
        if (price <= 0)
        {
            throw new ArgumentException("Price must be positive.");
        }

        Price = price;
    }

    public void IncreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.");
        }

        AvailableStock += quantity;
    }

    public void DecreaseStock(int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive.");
        }

        if (AvailableStock < quantity)
        {
            throw new InvalidOperationException("Insufficient stock.");
        }

        AvailableStock -= quantity;
    }
}
