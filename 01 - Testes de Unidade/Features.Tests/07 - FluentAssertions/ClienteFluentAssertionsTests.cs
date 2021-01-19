using FluentAssertions;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteFluentAssertionsTests
    {
        private readonly ClienteTestsAutoMockerFixture ClienteTestsFixture;

        public ClienteFluentAssertionsTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            ClienteTestsFixture = clienteTestsFixture;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Fluent Assertion Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            //Arrange
            var cliente = ClienteTestsFixture.GerarClienteValido();

            //Act
            var result = cliente.EhValido();

            //Assert
            //Assert.True(result);
            //Assert.Equal(0, cliente.ValidationResult.Errors.Count);

            //Assert
            result.Should().BeTrue();
            cliente.ValidationResult.Errors.Should().HaveCount(0);
        }
        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Fluent Assertion Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            //Arrange
            var cliente = ClienteTestsFixture.GerarClienteInvalido();

            //Act
            var result = cliente.EhValido();

            //Assert
            //Assert.False(result);
            //Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);

            //Assert
            result.Should().BeFalse();
            cliente.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1,"deve possuir erros de validação");
        }
    }
}