using FluentAssertions;
using Vendas.Domain.Clientes.Entities;
using Vendas.Domain.Common.Exceptions;

namespace Vendas.Domain.Tests.Clientes.Entities
{
    public class EnderecoTest
    {
        private static Endereco CriarEnderecoValido()
        {
            return new Endereco(
                cep: "12345678",
                logradouro: "Rua Exemplo",
                numero: "123",
                complemento: "",
                bairro: "Bairro Exemplo",
                cidade: "Cidade Exemplo",
                estado: "Estado Exemplo",
                pais: "País Exemplo"
            );
        }

        [Fact(DisplayName = "Deve criar um endereço válido.")]
        public void Deve_Criar_Endereco_Vaido()
        {
            // Arrange & Act
            var endereco = CriarEnderecoValido();
            // Assert
            endereco.Cep.Should().Be("12345678");
            endereco.Logradouro.Should().Be("Rua Exemplo");
            endereco.Numero.Should().Be("123");
            endereco.Complemento.Should().BeEmpty();
            endereco.Bairro.Should().Be("Bairro Exemplo");
            endereco.Cidade.Should().Be("Cidade Exemplo");
            endereco.Estado.Should().Be("Estado Exemplo");
            endereco.Pais.Should().Be("País Exemplo");
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Deve_Lancar_Excecao_Quando_Cep_Invalido(string? cepInvalido)
        {
            // Arrange & Act
            Action act = () => new Endereco(
                cep: cepInvalido!,
                logradouro: "Rua Exemplo",
                numero: "123",
                complemento: "",
                bairro: "Bairro Exemplo",
                cidade: "Cidade Exemplo",
                estado: "Estado Exemplo",
                pais: "País Exemplo"
                );

            // Assert
            act.Should().Throw<DomainException>().WithMessage("O CEP é obrigatório.");
        }

        [Fact(DisplayName = "Deve lançar erro quanto o cep não tiver 8 digitos.")]
        public void Deve_Lancar_Excecao_Quando_Cep_Tiver_Digitos_Invalidos()
        {
            // Arrange & Act
            Action act = () => new Endereco(
                cep: "1234-567",
                logradouro: "Rua Exemplo",
                numero: "123",
                complemento: "",
                bairro: "Bairro Exemplo",
                cidade: "Cidade Exemplo",
                estado: "Estado Exemplo",
                pais: "País Exemplo"
                );

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Cep inválido.");

        }

        [Theory(DisplayName = "Deve lançar erro quando campos obrigatórios forem inválidos.")]
        [InlineData(null, "123", "", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "País Exemplo")]
        [InlineData("Rua Exemplo", null, "", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", "País Exemplo")]
        [InlineData("Rua Exemplo", "123", "", null, "Cidade Exemplo", "Estado Exemplo", "País Exemplo")]
        [InlineData("Rua Exemplo", "123", "", "Bairro Exemplo", null, "Estado Exemplo", "País Exemplo")]
        [InlineData("Rua Exemplo", "123", "", "Bairro Exemplo", "Cidade Exemplo", null, "País Exemplo")]
        [InlineData("Rua Exemplo", "123", "", "Bairro Exemplo", "Cidade Exemplo", "Estado Exemplo", null)]
        public void Deve_Lancar_Excecao_Quando_Campos_Obrigatorios_Sao_Invalidos(
            string? logradouro,
            string? numero,
            string complemento,
            string? bairro,
            string? cidade,
            string? estado,
            string? pais)
        {
            // Arrange & Act
            Action act = () => new Endereco(
                cep: "12345678",
                logradouro: logradouro!,
                numero: numero!,
                complemento: complemento,
                bairro: bairro!,
                cidade: cidade!,
                estado: estado!,
                pais: pais!
                );

            // Assert
            act.Should().Throw<DomainException>();
        }

        [Fact(DisplayName = "Deve atualizar um endereço com dados válidos.")]
        public void Deve_Atualizar_Endereco_Com_Dados_Validos()
        {
            // Arrange
            var endereco = CriarEnderecoValido();
            // Act
            endereco.Atualizar(
                cep: "87654321",
                logradouro: "Avenida Exemplo",
                numero: "456",
                complemento: "Apto 789",
                bairro: "Bairro Exemplo 2",
                cidade: "Cidade Exemplo 2",
                estado: "Estado Exemplo 2",
                pais: "País Exemplo 2"
            );
            // Assert
            endereco.Cep.Should().Be("87654321");
            endereco.Logradouro.Should().Be("Avenida Exemplo");
            endereco.Numero.Should().Be("456");
            endereco.Complemento.Should().Be("Apto 789");
            endereco.Bairro.Should().Be("Bairro Exemplo 2");
            endereco.Cidade.Should().Be("Cidade Exemplo 2");
            endereco.Estado.Should().Be("Estado Exemplo 2");
            endereco.Pais.Should().Be("País Exemplo 2");
        }

        [Fact(DisplayName = "Deve lançar erro ao atualizar endereço com cep inválido.")]
        public void Deve_Lancar_Excecao_Ao_Atualizar_Endereco_Com_Cep_Invalido()
        {
            // Arrange
            var endereco = CriarEnderecoValido();
            // Act
            Action act = () => endereco.Atualizar(
                cep: "invalid-cep",
                logradouro: "Avenida Exemplo",
                numero: "456",
                complemento: "Apto 789",
                bairro: "Bairro Exemplo 2",
                cidade: "Cidade Exemplo 2",
                estado: "Estado Exemplo 2",
                pais: "País Exemplo 2"
            );

            // Assert
            act.Should().Throw<DomainException>().WithMessage("Cep inválido.");
        }

        [Fact(DisplayName = "Deve lançar erro ao atualizar endereço com campo obrigatório inválido.")]
        public void Deve_Lancar_Excecao_Ao_Atualizar_Endereco_Com_Campo_Obrigatotio_Invalido()
        {
            // Arrange
            var endereco = CriarEnderecoValido();
            // Act
            Action act = () => endereco.Atualizar(
                cep: "04194260",
                logradouro: "",
                numero: "456",
                complemento: "Apto 789",
                bairro: "Bairro Exemplo 2",
                cidade: "Cidade Exemplo 2",
                estado: "Estado Exemplo 2",
                pais: "País Exemplo 2"
            );

            // Assert
            act.Should().Throw<DomainException>();
        }
    }
}
