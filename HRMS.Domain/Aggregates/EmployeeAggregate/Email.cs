namespace HRMS.Domain.Aggregates.EmployeeAggregate;

public class Email
{
    public string Value { get; private set; }

    // EF Core requires a parameterless constructor
    private Email() { }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Email cannot be empty", nameof(value));

        // Optionally add regex/email validation here
        Value = value;
    }

    // Optional: Implicit conversion operators
    public static implicit operator string(Email email) => email.Value;
    public static implicit operator Email(string value) => new(value);

    // Value object equality
    public override bool Equals(object? obj)
    {
        if (obj is not Email other) return false;
        return Value == other.Value;
    }

    public override int GetHashCode() => Value.GetHashCode();
}