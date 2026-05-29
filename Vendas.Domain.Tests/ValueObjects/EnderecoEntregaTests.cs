using FluentAssertions;
using Vendas.Domain.Common.Exceptions;
using Vendas.Domain.Pedidos.ValueObjects;

namespace Vendas.Domain.Tests.ValueObjects
{
    public class EnderecoEntregaTests
    {
        [Fact(DisplayName = "Deve criar endereço de entrega com sucesso quando todos os dados são válidos")]
        public void Criar_DeveRetornarEnderecoValido_QuandoDadosForemValidos()
        {
            //Arrange
            var cep = "12345-678";
            var logradouro = "Rua das Flores";
            var complemento = "Apto 101";
            var bairro = "Centro";
            var cidade = "São Paulo";
            var estado = "SP";
            var pais = "Brasil";

            //Act
            var endereco = EnderecoEntrega.Criar(cep, logradouro, complemento, bairro, cidade, estado, pais);

            //Assert
            endereco.Should().NotBeNull();
            endereco.Cep.Should().Be(cep);
            endereco.Logradouro.Should().Be(logradouro);
            endereco.Complemento.Should().Be(complemento);
            endereco.FormatarEndereco().Should().Contain("Rua das Flores");
        }

        [Theory(DisplayName = "Deve lançar DomainException quando cep for inválido")]
        [InlineData("12345678")]
        [InlineData("12-345678")]
        [InlineData("ABCDE-123")]
        public void Criar_DeveLancarDomainException_QuandoCepForInvalido(string cepInvalido)
        {
            //Arrange
            var logradouro = "Rua das Flores";
            var complemento = "Apto 101";
            var bairro = "Centro";
            var cidade = "São Paulo";
            var estado = "SP";
            var pais = "Brasil";

            //Act
            Action act = () => EnderecoEntrega.Criar(cepInvalido, logradouro, complemento, bairro, cidade, estado, pais);

            //Assert
            act.Should().Throw<DomainException>().WithMessage("CEP inválido*");
        }

        [Fact(DisplayName = "Dois enderecos com mesmos valores deve ser iguais(Value Object)")]
        public void EnderecosDevemSerIguais_QuandoPossuemMesmosValores()
        {
            //arrange
            var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa 1", "Centro", "São Paulo", "SP", "Brasil");
            var endereco2 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa 1", "Centro", "São Paulo", "SP", "Brasil");

            //Assert
            endereco1.Should().Be(endereco2);
            (endereco1 == endereco2).Should().BeTrue();
        }

        [Fact(DisplayName = "Dois enderecos devem ser diferentes quando algum campo for diferente")]
        public void EnderecosDevemSerIguais_QuandoQuandoAlgumCampoForDiferente()
        {
            //arrange
            var endereco1 = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa 1", "Centro", "São Paulo", "SP", "Brasil");
            var endereco2 = EnderecoEntrega.Criar("12345-678", "Rua Y", "Casa 1", "Centro", "São Paulo", "SP", "Brasil");

            //Assert
            endereco1.Should().NotBe(endereco2);
        }

        [Fact(DisplayName = "Endereco de entrega deve ser imutável após criação")]
        public void EnderecoDeveSerImutavel_AposCriacao()
        {
            //Arrange
            var endereco = EnderecoEntrega.Criar("12345-678", "Rua X", "Casa 1", "Centro", "São Paulo", "SP", "Brasil");

            //Act
            Action act = () => 
            {
                //endereco.Cep = "99999-999";
            };

            //Assert
            endereco.GetType().GetProperties()
                .All(p => p.SetMethod == null || p.SetMethod.IsPrivate)
                .Should().BeTrue("as propriedades do VO devem ser imutáveis"); 

        }

        [Theory(DisplayName = "Deve lançar DomainException quando campos obrigatórios forem nulos ou vazios.")]
        [InlineData(null, "Logradouro", "Bairro", "Cidade", "Estado", "Pais")]
        [InlineData("12345678", null, "Bairro", "Cidade", "Estado", "Pais")]
        [InlineData("12345678", "Logradouro", "Bairro", "Cidade", "Estado", null)]
        public void Criar_DeveLancarDomainException_QuandoCampoObrigatorioNulosOuVazios(
                string cep, string logradouro, string bairro, string cidade, string estado, string pais)
        {
            
            //Act
            Action act = () => EnderecoEntrega.Criar(cep, logradouro, "complemento", bairro, cidade, estado, pais);

            //Assert
            act.Should().Throw<DomainException>().WithMessage("*não pode ser nulo ou vazio*");
        }

    }
}
