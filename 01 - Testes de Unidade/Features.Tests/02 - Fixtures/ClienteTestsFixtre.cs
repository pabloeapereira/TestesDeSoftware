using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteCollection))]
    public class ClienteCollection : ICollectionFixture<ClienteTestsFixtre>
    {}
    public class ClienteTestsFixtre:IDisposable
    {
        public Cliente GerarClienteValido() =>
            new Cliente(
                Guid.NewGuid(),
                "Pablo",
                "Emanuel",
                DateTime.Now.AddYears(-30),
                "pablo@pablo.com.br",
                true,
                DateTime.Now);

        public Cliente GerarClienteInvalido() =>
            new Cliente(
                Guid.NewGuid(),
                string.Empty,
                string.Empty,
                DateTime.Now,
                "pablo2pablo.com.br",
                true,
                DateTime.Now);

        public void Dispose()
        {
        }
    }
}