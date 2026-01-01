namespace CleanArchitecture.Domain.Shared;

public record Currency(decimal Value, CurrencyType CurrencyType)
{
    public static Currency operator +(Currency first, Currency second)
    {
        return first.CurrencyType != second.CurrencyType
            ? throw new ApplicationException("Currencies must be the same type")
            : new Currency(first.Value + second.Value, first.CurrencyType);
    }

    public static Currency Zero() => new(0, CurrencyType.None);

    public static Currency Zero(CurrencyType currencyType) => new(0, currencyType);

    public bool IsZero() => this == Zero(CurrencyType);
};
