using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class Fee : ValueObject
{
    public float Amount { get; private set; }

    public string Currency { get; private set; }

    private Fee(float amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Fee Create(float amount, string? currency)
    {
        return new Fee(amount, currency ?? Shared.Courses.Currency.USD);
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}