using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Rentals;

public record class PriceDetail(
    Currency PricePerPeriod,
    Currency PriceMaintenance,
    Currency PriceAccessories,
    Currency TotalPrice
);
