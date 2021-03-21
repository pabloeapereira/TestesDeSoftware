using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteAutoMockerCollection))]
    public class ClienteAutoMockerCollection : ICollectionFixture<ClienteTestsAutoMockerFixture>
    { }
    public class ClienteTestsAutoMockerFixture : IDisposable
    {
        public ClienteService ClienteService;
        public AutoMocker AutoMocker;
        public Cliente GerarClienteValido() =>
            GerarClientes(1, true).FirstOrDefault();

        public IEnumerable<Cliente> ObterClientesVariados()
        {
            var clientes = new List<Cliente>();
            clientes.AddRange(GerarClientes(50, true));
            clientes.AddRange(GerarClientes(50, false));
            return clientes;
        }

        public IEnumerable<Cliente> GerarClientes(int quantidade, bool ativo)
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            var clientes = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    string.Empty,
                    ativo,
                    DateTime.Now))
                .RuleFor(x => x.Email, (f, c) => f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return clientes.Generate(quantidade);
        }

        public Cliente GerarClienteInvalido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();

            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(1, DateTime.Now.AddYears(1)),
                    string.Empty,
                    false,
                    DateTime.Now));

            return cliente;
        }

        public void Dispose()
        {
        }

        public ClienteService ObterClienteService()
        {
            AutoMocker = new AutoMocker();
            ClienteService = AutoMocker.CreateInstance<ClienteService>();
            return ClienteService;
        }
    }
}
