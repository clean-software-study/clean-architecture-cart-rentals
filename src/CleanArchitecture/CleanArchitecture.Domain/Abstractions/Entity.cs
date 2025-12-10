namespace CleanArchitecture.Domain.Abstractions;

public abstract class Entity(Guid id)
{

    private readonly List<IDomainEvent> _domainEvents = [];

    // Id init: significa que una vez inicializado, no se puede cambiar
    public Guid Id { get; init; } = id;


    public IReadOnlyList<IDomainEvent> GetDomainEvents()
    {
        return _domainEvents.ToList();
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    protected void RaisedDomainEvent(IDomainEvent domainEvents)
    {
        _domainEvents.Add(domainEvents);
    }
}