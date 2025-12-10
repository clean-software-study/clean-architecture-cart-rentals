using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Vehicles;

public sealed class Vehicle(
    Guid id,
    Model? model,
    Vin? vin = null,
    Address? address = null,
    Currency? price = null,
    Currency? maintenance = null,
    List<Accessory>? accessories = null
) : Entity(id)
{
    public Model? Model { get; private set; } = model;
    public Vin? Vin { get; private set; } = vin;
    public Address? Address { get; private set; } = address;
    public Currency? Price { get; private set; } = price;
    public Currency? Maintenance { get; private set; } = maintenance;
    public List<Accessory>? Accessories { get; private set; } = accessories;

}