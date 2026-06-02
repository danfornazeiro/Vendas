using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.ValueObjects
{
    public sealed record ValorMonetario
    {
        public decimal Valor { get; } 

        public ValorMonetario(decimal valor)
        {
            Guard.Against<DomainException>(valor <= 0, "O Valor do pagamento deve ser maior que zero.");

            Valor = valor;
        }
    }
}
