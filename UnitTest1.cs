using Microsoft.Extensions.Hosting;
using PactNet;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace PactExample
{
    public class UnitTest1
    {
        private readonly IPactBuilderV3 _builder;

        public UnitTest1()
        {
            var pact = Pact.V3("consumerTesting", "pokeAPI", new PactConfig());

            this._builder = pact.UsingNativeBackend();
        }

        [Fact(DisplayName = "Exemplo de teste consumer")]
        public async Task exemploConsumer()
        {
            var random = new Random().Next();

            this._builder
                .UponReceiving("Um get de um endpoint")
                .Given("Que os dados venham corretamente")
                .WithRequest(HttpMethod.Get, "https://pokeapi.co/api/v2/pokemon/gengar")
                .WithHeader("Accept", "application/json")
           .WillRespond()
                .WithStatus(System.Net.HttpStatusCode.OK)
                .WithJsonBody(new
                {
                    id = random,
                    name = "gengar",
                    is_default = true
                });

            await this._builder.VerifyAsync(async ctx =>
            {
                var client = new HttpClient();

                var teste = await client.GetAsync("https://pokeapi.co/api/v2/pokemon/gengar");

                //Assert
                Assert.NotNull(teste);
            });
        }
    }
}
