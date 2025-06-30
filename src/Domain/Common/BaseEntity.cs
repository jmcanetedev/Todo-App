using System.ComponentModel.DataAnnotations.Schema;

namespace Todo_App.Domain.Common;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? DeletedOn { get; private set; }

    private readonly List<BaseEvent> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
    public void SoftDeleteEntity()
    {
        DeletedOn = DateTime.UtcNow;
    }
}
