using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.EntityFrameworkCore;
using Sprint1.Controllers;
using Sprint1.Data;
using Sprint1.Models;
using Xunit;

namespace Sprint1.Tests.Unit
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task Index_BuscaProdutos_RetornaSeisProdutosOrdenadosPorId()
        {
            var loggerMock = new Mock<ILogger<HomeController>>();
            var contextMock = new Mock<ApplicationDbContext>();

            var produtosFalsos = new List<Produto>();
            for (int i = 1; i <= 10; i++)
            {
                produtosFalsos.Add(new Produto
                {
                    Produto_ID = i,
                    Titulo = $"Mangá {i}",
                    Preco_original = 10,
                    Descricao = "Descrição",
                    Imagem_url = "http://img.jpg"
                });
            }

            contextMock.Setup(c => c.Produtos).ReturnsDbSet(produtosFalsos);

            var controller = new HomeController(loggerMock.Object, contextMock.Object);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Produto>>(viewResult.Model);

            Assert.Equal(6, model.Count());
            Assert.Equal(10, model.First().Produto_ID);
        }
    }
}