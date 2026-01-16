using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehicles;

namespace CleanArchitecture.Domain.Rentals.DomainServices;

public class PriceService
{
    public PriceDetail CalculatePrice(Vehicle vehicle, DateRange rentalPeriod)
    {
        var currency = vehicle.Price!.CurrencyType;

        var baseRentalPrice = new Currency(
            rentalPeriod.AmountOfDays() * vehicle.Price.Value,
            currency
        );

        var accessoriesSurcharge = CalculateAccessoriesSurcharge(
            vehicle.Accessories,
            baseRentalPrice
        );

        var feeMaintenance = vehicle.Maintenance ?? Currency.Zero(currency);
        var totalPrice = baseRentalPrice + accessoriesSurcharge + feeMaintenance;

        return new PriceDetail(baseRentalPrice, feeMaintenance, accessoriesSurcharge, totalPrice);
    }

    private Currency CalculateAccessoriesSurcharge(
        IEnumerable<Accessory> accessories,
        Currency basePrice
    )
    {
        var surchargePercentage = accessories.Sum(accessory =>
            accessory switch
            {
                Accessory.AppleCart or Accessory.AndroiCart => 0.05m,
                Accessory.AirConditioning => 0.01m,
                Accessory.Maps => 0.01m,
                _ => 0,
            }
        );

        return surchargePercentage > 0
            ? new Currency(basePrice.Value * surchargePercentage, basePrice.CurrencyType)
            : Currency.Zero(basePrice.CurrencyType);
    }
}
