namespace ModularMonolith.Modules.Users.Domain;

internal sealed class User
{
    private User()
    {
    }

    public Guid Id { get; private set; }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string Email { get; private set; } = string.Empty;

    public bool IsActive { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public static User Create(string firstName, string lastName, string email)
    {
        if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Names are required.");
        }

        if (!email.Contains('@'))
        {
            throw new ArgumentException("A valid email is required.");
        }

        return new User
        {
            Id = Guid.NewGuid(),
            FirstName = firstName.Trim(),
            LastName = lastName.Trim(),
            Email = email.Trim(),
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

    public void ChangeEmail(string email)
    {
        if (!email.Contains('@'))
        {
            throw new ArgumentException("A valid email is required.");
        }

        Email = email.Trim();
    }
}
