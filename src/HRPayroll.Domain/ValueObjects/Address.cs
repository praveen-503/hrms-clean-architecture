namespace HRPayroll.Domain.ValueObjects;

/// <summary>
/// Value object representing a physical address.
/// </summary>
public record Address(
    string Street,
    string City,
    string State,
    string Country,
    string ZipCode);
