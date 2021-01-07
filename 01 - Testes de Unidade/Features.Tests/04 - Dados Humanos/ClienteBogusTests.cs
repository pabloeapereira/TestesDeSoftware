using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteBogusCollection))]
    public class ClienteBogusTests
    {
        private readonly ClienteTestsBogusFixture ClienteTestsFixture;

        public ClienteBogusTests(ClienteTestsBogusFixture clienteTestsFixture)
        {
            ClienteTestsFixture = clienteTestsFixture;
        }

        [Fact(DisplayName = "Novo Cliente Válido")]
        [Trait("Categoria", "Cliente Bogus Testes")]
        public void Cliente_NovoCliente_DeveEstarValido()
        {
            //Arrange
            var cliente = ClienteTestsFixture.GerarClienteValido();

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.True(result);
            Assert.Equal(0, cliente.ValidationResult.Errors.Count);
        }
        [Fact(DisplayName = "Novo Cliente Inválido")]
        [Trait("Categoria", "Cliente Bogus Testes")]
        public void Cliente_NovoCliente_DeveEstarInvalido()
        {
            //Arrange
            var cliente = ClienteTestsFixture.GerarClienteInvalido();

            //Act
            var result = cliente.EhValido();

            //Assert
            Assert.False(result);
            Assert.NotEqual(0, cliente.ValidationResult.Errors.Count);
        }
    }
}