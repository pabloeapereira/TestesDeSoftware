using Features.Clientes;
using MediatR;
using Moq;
using System.Linq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceAutoMockerFixtureTests
    {
        private readonly ClienteTestsAutoMockerFixture ClienteTestsAutoMockerFixture;
        private readonly ClienteService ClienteService;

        public ClienteServiceAutoMockerFixtureTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            ClienteTestsAutoMockerFixture = clienteTestsFixture;
            ClienteService = clienteTestsFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service AutoMock Fixture Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = ClienteTestsAutoMockerFixture.GerarClienteValido();

            // Act
            ClienteService.Adicionar(cliente);

            // Assert
            Assert.True(cliente.EhValido());
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>()
                .Verify(r => r.Adicionar(cliente), Times.Once);
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IMediator>()
                .Verify(m => m.Publish(It.IsAny<INotification>(), default), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service AutoMock Fixture Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = ClienteTestsAutoMockerFixture.GerarClienteInvalido();

            // Act
            ClienteService.Adicionar(cliente);

            // Assert
            Assert.False(cliente.EhValido());
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), default), Times.Never);
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service AutoMock Fixture Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(ClienteTestsAutoMockerFixture.ObterClientesVariados());


            // Act
            var clientes = ClienteService.ObterTodosAtivos();

            // Assert 
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c => !c.Ativo) > 0);
        }
    }
}