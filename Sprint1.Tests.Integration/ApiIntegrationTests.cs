using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Sprint1.Data;
using Xunit;
using System.Collections.Generic;

namespace Sprint1.Tests.Integration
{
    public class ApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ApiIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                // 1. Injeta configuração falsa para não quebrar validações de string vazia
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    config.AddInMemoryCollection(new Dictionary<string, string?>
                    {
                        { "ConnectionStrings:OracleConnection", "User Id=fake;Password=fake;Data Source=fake" }
                    });
                });

                builder.ConfigureTestServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                    services.RemoveAll(typeof(ApplicationDbContext));

                    var serviceProvider = new ServiceCollection()
                        .AddEntityFrameworkInMemoryDatabase()
                        .BuildServiceProvider();

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("BancoTesteIntegracao_Absoluto");
                        options.UseInternalServiceProvider(serviceProvider);
                    });
                });
            });
        }

        [Fact]
        public async Task GetHealthCheck_EndpointAcessivel_RetornaSucessoEContentTypeJson()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/health");

            response.EnsureSuccessStatusCode();
            Assert.Equal("application/json", response.Content.Headers.ContentType?.ToString());
        }

        [Fact]
        public async Task GetMangasIndex_PaginaAcessada_RetornaHtmlComSucesso()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("/mangas");

            response.EnsureSuccessStatusCode();
            Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType?.ToString());
        }
    }
}