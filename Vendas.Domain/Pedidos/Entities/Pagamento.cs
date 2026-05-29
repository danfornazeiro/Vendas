using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Enums;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;
using Vendas.Domain.Pedidos.Events;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Pedidos.Entities
{
    public sealed class Pagamento : Entity
    {
        public Guid PedidoId { get; private set; }
        public MetodoPagamento MetodoPagamento { get; private set; }
        public StatusPagamento StatusPagamento { get; private set; }
        public ValorMonetario Valor { get; private set; }
        public DateTime? DataPagamento { get; private set; }
        public string? CodigoTransacao { get; private set; }

        public Pagamento(Guid pedidoId, MetodoPagamento metodoPagamento, StatusPagamento statusPagamento, ValorMonetario valor)
        {
            Guard.AgainsEmptyGuid(pedidoId, nameof(pedidoId), "Pedido inválido.");
            Guard.Against<DomainException>(!Enum.IsDefined(typeof(MetodoPagamento), metodoPagamento), "Metodo de pagamento inválido");

            PedidoId = pedidoId;
            MetodoPagamento = metodoPagamento;
            Valor = valor;

            StatusPagamento = StatusPagamento.Pendente;
            DataPagamento = null;
            CodigoTransacao = null;
        }

        public void GerarCodigoTransacaoLocal()
        {
            if (CodigoTransacao is not null) return;

            var codigo = $"LOCAL-{Guid.NewGuid().ToString()[..8].ToUpper()}";

            DefinirCodigoTransacao(codigo);
        }

        private void DefinirCodigoTransacao(string codigo)
        {
            Guard.AgainstNullOrWhiteSpace(codigo, nameof(codigo), "Código da transacao inválido");
            Guard.Against<DomainException>(CodigoTransacao is null, "O código da transação já foi definido.");
            Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Não é permitido registrar código após confirmação ou recusa do pagamento.");

            CodigoTransacao = codigo;
            SetDataAtualizacao();
        }

        public void ConfirmarPagamento(string codigo)
        {
            Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Apenas pagamentos pendentes podem ser confirmados.");

            Guard.AgainstNullOrWhiteSpace(CodigoTransacao ?? string.Empty, nameof(CodigoTransacao), "O pagamento nãp pode ser confirmado sem o código de transação.");

            StatusPagamento = StatusPagamento.Aprovado; 
            DataPagamento = DateTime.UtcNow;
            SetDataAtualizacao();

            AddDomainEvent(new PagamentoAprovadoEvent(
                Id,
                PedidoId,
                Valor,
                DataPagamento.Value,
                CodigoTransacao));
        }

        public void RecusarPagamento(string codigo)
        {
            Guard.Against<DomainException>(StatusPagamento != StatusPagamento.Pendente, "Apenas pagamentos pendentes podem ser recusados.");

            Guard.AgainstNullOrWhiteSpace(CodigoTransacao ?? string.Empty, nameof(CodigoTransacao), "O pagamento nãp pode ser confirmado sem o código de transação.");

            StatusPagamento = StatusPagamento.Aprovado;
            DataPagamento = DateTime.UtcNow;
            SetDataAtualizacao();

            AddDomainEvent(new PagamentoRejeitadoEvent(
                Id,
                PedidoId,
                Valor,
                DataPagamento.Value,
                CodigoTransacao));
        }
    }
}
