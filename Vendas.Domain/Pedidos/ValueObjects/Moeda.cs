namespace Vendas.Domain.Pedidos.ValueObjects
{
    public sealed record Moeda
    {
        public decimal Valor { get; }

        public Moeda(decimal valor)
        {
            if (valor < 0)
            {
                throw new ArgumentException("O Valor não pode ser menor que 0!");
            }

            this.Valor = valor;
        }
    }
}
