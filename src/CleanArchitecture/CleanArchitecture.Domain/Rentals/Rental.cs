using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Rentals.DomainServices;
using CleanArchitecture.Domain.Shared;
using CleanArchitecture.Domain.Vehicles;

namespace CleanArchitecture.Domain.Rentals;

public class Rental : Entity
{
    public Guid VegihicleId { get; private set; }
    public Guid UserId { get; private set; }
    public RentalStatus Status { get; private set; }
    public Currency? PricePerPeriod { get; private set; }
    public Currency? PriceMaintenance { get; private set; }
    public Currency? PriceAccessories { get; private set; }
    public Currency? TotalPrice { get; private set; }
    public DateRange? Duration { get; private set; }
    public DateTime? DateCreated { get; private set; }
    public DateTime? DateConfirmation { get; private set; }
    public DateTime? DateDenied { get; private set; }
    public DateTime? DateCompleted { get; private set; }
    public DateTime? DateCalled { get; private set; }

    // Constructor privado
    private Rental(
        Guid id,
        Guid userId,
        Guid vegihicleId,
        RentalStatus status,
        Currency? pricePerPeriod,
        Currency? priceMaintenance,
        Currency? priceAccessories,
        Currency? totalPrice,
        DateRange? duration,
        DateTime? dateCreated
    )
        : base(id)
    {
        VegihicleId = vegihicleId;
        UserId = userId;
        Status = status;
        PricePerPeriod = pricePerPeriod;
        PriceMaintenance = priceMaintenance;
        PriceAccessories = priceAccessories;
        TotalPrice = totalPrice;
        Duration = duration;

        DateCreated = dateCreated;
    }

    public static Rental Reserve(
        Vehicle vehicle,
        Guid userId,
        DateRange duration,
        DateTime dateCreated,
        PriceService priceService
    )
    {
        var priceDetail = priceService.CalculatePrice(vehicle, duration);
        return new Rental(
            Guid.NewGuid(),
            userId,
            vehicle.Id,
            RentalStatus.Reserved,
            priceDetail.PricePerPeriod,
            vehicle.Maintenance,
            priceDetail.PriceAccessories,
            priceDetail.TotalPrice,
            duration,
            dateCreated
        );
    }
}
