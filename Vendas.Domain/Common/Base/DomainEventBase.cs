using Vendas.Domain.Common.Interfaces;

namespace Vendas.Domain.Common.Base;

public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOcurred { get; protected set; } = DateTime.UtcNow;
}
