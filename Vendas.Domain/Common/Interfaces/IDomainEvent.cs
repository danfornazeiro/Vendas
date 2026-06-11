namespace Vendas.Domain.Common.Interfaces
{
    public interface IDomainEvent
    {
        DateTime DateOcurred { get; }
    }
}
