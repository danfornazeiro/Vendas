using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Events
{
    public record PagamentoAprovadoEvent(
        Guid PagamentoId,
        Guid PedidoId,
        ValorMonetario Valor,
        DateTime DataPagamento,
        string? CodigoTransacao) : DomainEventBase;

}
