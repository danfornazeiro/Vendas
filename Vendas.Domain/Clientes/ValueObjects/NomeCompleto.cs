using System.Runtime.CompilerServices;
using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Clientes.ValueObjects
{
    public sealed class NomeCompleto : ValueObject
    {
        public string Nome { get; }
        public string Sobrenome { get; set; }
        public string NomeCompletoFormatado { get; set; }

        public NomeCompleto(string nomeCompleto)
        {
            Guard.AgainstNullOrWhiteSpace(nomeCompleto, nameof(nomeCompleto), "O nome completo é obrigatório.");

            var partes = nomeCompleto.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Guard.Against<DomainException>(partes.Length < 2, "O nome completo deve conter pelo menos nome e sobrenome.");

            Sobrenome = partes.Last();
            Nome = string.Join(" ", partes.Take(partes.Length - 1));

            NomeCompletoFormatado = string.Join(" ", partes);
        }

        public string NomeResumido => $"{Nome.Split(' ').First()} {Sobrenome}";

        public override string ToString() => NomeCompletoFormatado;

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return NomeCompletoFormatado.ToLowerInvariant();
        }
    }
}
