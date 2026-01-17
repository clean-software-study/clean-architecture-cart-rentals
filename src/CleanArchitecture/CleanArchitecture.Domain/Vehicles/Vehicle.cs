using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Shared;

namespace CleanArchitecture.Domain.Vehicles;

public sealed class Vehicle(
    Guid id,
    Model? model,
    Vin? vin,
    Address? address,
    Currency? price,
    Currency? maintenance,
    List<Accessory> accessories
) : Entity(id)
{
    public Model? Model { get; private set; } = model;
    public Vin? Vin { get; private set; } = vin;
    public Address? Address { get; private set; } = address;
    public Currency? Price { get; private set; } = price;
    public Currency? Maintenance { get; private set; } = maintenance;
    public List<Accessory> Accessories { get; private set; } = accessories;
}
