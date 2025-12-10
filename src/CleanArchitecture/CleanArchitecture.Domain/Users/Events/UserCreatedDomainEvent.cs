using CleanArchitecture.Domain.Abstractions;

namespace CleanArchitecture.Domain.Users.Events;

public record UserCreatedDomainEvent(Guid UserId): IDomainEvent
{
    
}
