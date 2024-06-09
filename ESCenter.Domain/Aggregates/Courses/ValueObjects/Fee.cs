using Matt.SharedKernel.Domain.Primitives;

namespace ESCenter.Domain.Aggregates.Courses.ValueObjects;

public class Fee : ValueObject
{
    public decimal Amount { get; private set; }

    public string Currency { get; private set; } = Shared.Courses.Currency.USD;

    private Fee()
    {
    }

    public static Fee Create(decimal amount, string? currency)
    {
        return new Fee()
        {
            Amount = amount,
            Currency = currency ?? Shared.Courses.Currency.USD
        };
    }

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }
}