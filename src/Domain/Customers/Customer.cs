namespace Domain.Customers;

public sealed class Customer
{
    public Guid Id { get; private set; }
    public string Email { get; private set; } = default!;

    private Customer() { }

    public Customer(Guid id, string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("invalid email", nameof(email));

        Id = id;
        Email = email.Trim().ToLowerInvariant();
    }
}
