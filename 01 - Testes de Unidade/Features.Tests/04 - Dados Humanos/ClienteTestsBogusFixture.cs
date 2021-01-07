using Bogus;
using Bogus.DataSets;
using Features.Clientes;
using System;
using Xunit;

namespace Features.Tests
{
    [CollectionDefinition(nameof(ClienteBogusCollection))]
    public class ClienteBogusCollection : ICollectionFixture<ClienteTestsBogusFixture>
    { }
    public class ClienteTestsBogusFixture:IDisposable
    {
        public Cliente GerarClienteValido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            /*var clienteFaker = new Faker<Cliente>();
            clienteFaker.RuleFor(x => x.Nome, s => s.Name.FirstName());*/
            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(80, DateTime.Now.AddYears(-18)),
                    string.Empty,
                    true,
                    DateTime.Now))
                .RuleFor(x => x.Email, (f, c) => f.Internet.Email(c.Nome.ToLower(), c.Sobrenome.ToLower()));

            return cliente;
        }


        public Cliente GerarClienteInvalido()
        {
            var genero = new Faker().PickRandom<Name.Gender>();
            
            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    Guid.NewGuid(),
                    f.Name.FirstName(genero),
                    f.Name.LastName(genero),
                    f.Date.Past(1,DateTime.Now.AddYears(1)),
                    string.Empty,
                    false,
                    DateTime.Now));

            return cliente;
        }

        public void Dispose()
        {
        }
    }
}