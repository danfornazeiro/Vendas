using Vendas.Domain.Common.Base;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Common.Validations;

namespace Vendas.Domain.Pedidos.Entities
{
    public sealed class ItemPedido : Entity
    {
        public Guid ProdutoId { get; private set; }
        public string NomeProduto { get; private set; } = string.Empty;
        public decimal PrecoUnitario { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorTotal { get; private set; }
        public decimal DescontoAplicado { get; private set; }

        internal ItemPedido(
            Guid produtoId,
            string nomeProduto,
            decimal precoUnitario,
            int quantidade) 
        {
            Guard.AgainsEmptyGuid(produtoId, nameof(produtoId), "ProdutoId inválido.");
            Guard.AgainstNullOrWhiteSpace(nomeProduto, nameof(nomeProduto), "O nome do produto é obrigatório.");
            Guard.Against<DomainException>(precoUnitario <= 0, "O preço unitário deve ser maior que zero.");
            Guard.Against<DomainException>(quantidade <= 0, "A quantidade deve ser maior que zero.");

            ProdutoId = produtoId;
            NomeProduto = nomeProduto;
            PrecoUnitario = precoUnitario;
            Quantidade = quantidade;
            DescontoAplicado = 0;

            CalcularValorTotal();
        }

        public void AplicarDesconto(decimal desconto)
        {
            Guard.Against<DomainException>(desconto < 0, "Desconto não pode ser negativo.");
            Guard.Against<DomainException>(desconto > PrecoUnitario * Quantidade, "Desconto não pode exceder o valor total do item.");

            DescontoAplicado = desconto;
            SetDataAtualizacao();
            CalcularValorTotal();
        }

        public void AdicionarUnidades(int unidades)
        {
            Guard.Against<DomainException>(unidades <= 0, "Deve-se adicionar pelo menos uma unidade.");
            Quantidade += unidades;
            SetDataAtualizacao();
            CalcularValorTotal();
        }

        public void RemoverUnidades(int unidades)
        {
            Guard.Against<DomainException>(unidades <= 0, "Deve-se remover pelo menos uma unidade");
            Guard.Against<DomainException>(unidades > Quantidade, "Não é possível remover mais unidades do que existem no item.");
            
            Quantidade -= unidades;

            Guard.Against<DomainException>(Quantidade <= 0, "Um item não pode ter quantidade menor ou igual a 0. Remova o item ou adicione pelo menos um item.");

            SetDataAtualizacao();
            CalcularValorTotal();
        }

        public void AtualizarPrecoUnitario(decimal novoPreco)
        {
            Guard.Against<DomainException>(novoPreco <= 0, "O preço unitário deve ser maior que zero.");

            PrecoUnitario = novoPreco;
            SetDataAtualizacao();
            CalcularValorTotal();
        }

        private void CalcularValorTotal()
        {
            ValorTotal = (PrecoUnitario * Quantidade) - DescontoAplicado;
        }
    }
}
