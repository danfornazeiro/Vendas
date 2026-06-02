using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.ValueObjects
{
    public sealed class MotivoCancelamento : ValueObject
    {
        public string Codigo { get; }
        public string Descricao { get; }

        private static readonly Dictionary<string, string> _motivosPadrao = new()
        {
            {"ClienteDesistiu", "Cliente desistiu da copmra" },
            {"ErroPagamento", "Erro no processamento do pagamento" },
            {"ItemSemEstoque", "Item esgotado no estoque" },
            {"EnderecoInvalido", "Endereço de entrega inválido" },
            {"Outro", "Outro motivo não especificado" }
        };

        public MotivoCancelamento(string codigo)
        {
            if (string.IsNullOrWhiteSpace(codigo)) throw new DomainException("O código do motivo de cancelamento é obrigatório.");
                
            if (!_motivosPadrao.ContainsKey(codigo)) throw new DomainException($"O motivo de cancelamento '{codigo}' não é válido.");

            Codigo = codigo;
            Descricao = _motivosPadrao[codigo];
        }

        public static MotivoCancelamento ClienteDesistiu() => new("ClienteDesistiu");
        public static MotivoCancelamento ErroPagamento() => new("ErroPagamento");
        public static MotivoCancelamento ItemSemEstoque() => new("ItemSemEstoque");
        public static MotivoCancelamento EnderecoInvalido() => new("EnderecoInvalido");
        public static MotivoCancelamento Outro() => new("Outro");

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Codigo;
            yield return Descricao;
        }
        public override string ToString() => $"{Descricao}";
        
    }
}
