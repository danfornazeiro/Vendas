using Vendas.Domain.Common.Enums;
using Vendas.Domain.ValueObjects;

namespace Vendas.Domain.Pedidos.Events;

public sealed record PedidoCanceladoEvent(
    Guid PedidoId,
    Guid ClienteId,
    StatusPedido StatusAnterior,
    MotivoCancelamento Motivo,
    Guid? PagamentoId) : DomainEventBase;

