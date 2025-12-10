
namespace CleanArchitecture.Domain.Vehicles;

public record CurrencyType
{
    public static readonly CurrencyType Usd = new("USD");
    public static readonly CurrencyType Eur = new("EUR");
    public static readonly CurrencyType None = new("");
    public static readonly IReadOnlyCollection<CurrencyType> All = [Usd, Eur, None];

    public static CurrencyType? FromCode(string code) => All.FirstOrDefault(x => x.Code == code)
        ?? throw new ApplicationException($"Currency type with code {code} not found");

    public CurrencyType(string code) => Code = code;
    public string? Code { get; set; }

}