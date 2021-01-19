using Features.Clientes;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(ClienteAutoMockerCollection))]
    public class ClienteServiceFluentAssertionTests
    {
        private readonly ClienteTestsAutoMockerFixture ClienteTestsAutoMockerFixture;
        private readonly ClienteService ClienteService;

        public ClienteServiceFluentAssertionTests(ClienteTestsAutoMockerFixture clienteTestsFixture)
        {
            ClienteTestsAutoMockerFixture = clienteTestsFixture;
            ClienteService = clienteTestsFixture.ObterClienteService();
        }

        [Fact(DisplayName = "Adicionar Cliente com Sucesso")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_Adicionar_DeveExecutarComSucesso()
        {
            // Arrange
            var cliente = ClienteTestsAutoMockerFixture.GerarClienteValido();

            // Act
            ClienteService.Adicionar(cliente);

            // Assert
            //Assert.True(cliente.EhValido());
            cliente.EhValido().Should().BeTrue();

            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>()
                .Verify(r => r.Adicionar(cliente), Times.Once);
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IMediator>()
                .Verify(m => m.Publish(It.IsAny<INotification>(), default), Times.Once);
        }

        [Fact(DisplayName = "Adicionar Cliente com Falha")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_Adicionar_DeveFalharDevidoClienteInvalido()
        {
            // Arrange
            var cliente = ClienteTestsAutoMockerFixture.GerarClienteInvalido();

            // Act
            ClienteService.Adicionar(cliente);

            // Assert
            //Assert.False(cliente.EhValido());
            cliente.EhValido().Should().BeFalse("Possui inconsistências");

            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Verify(r => r.Adicionar(cliente), Times.Never);
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), default), Times.Never);
        }

        [Fact(DisplayName = "Obter Clientes Ativos")]
        [Trait("Categoria", "Cliente Service Fluent Assertion Tests")]
        public void ClienteService_ObterTodosAtivos_DeveRetornarApenasClientesAtivos()
        {
            // Arrange
            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Setup(c => c.ObterTodos())
                .Returns(ClienteTestsAutoMockerFixture.ObterClientesVariados());


            // Act
            var clientes = ClienteService.ObterTodosAtivos();

            // Assert 
            //Assert.True(clientes.Any());
            //Assert.False(clientes.Count(c => !c.Ativo) > 0);

            // Assert
            clientes.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            clientes.Should().NotContain(c => !c.Ativo);


            ClienteTestsAutoMockerFixture.AutoMocker.GetMock<IClienteRepository>().Verify(r => r.ObterTodos(), Times.Once);

            ClienteService.ExecutionTimeOf(c => c.ObterTodosAtivos())
                .Should().BeLessOrEqualTo(50.Milliseconds(),"é executado milhares de vezes por segundo");
            
        }
    }
}