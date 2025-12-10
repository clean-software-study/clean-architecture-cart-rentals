namespace CleanArchitecture.Domain.Vehicles;

public interface IVehicleRepository
{
    Task<Vehicle?> GetVehicleAsync(Guid id, CancellationToken cancellationToken = default);
}
