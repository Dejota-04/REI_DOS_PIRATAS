using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sprint1.Data;
using Sprint1.Models;
using Sprint1.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

namespace Sprint1.Controllers
{
    [Route("mangas")]
    public class ProdutosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProdutosController(ApplicationDbContext context)
        {
            _context = context;
        }

        private void PopularDropdowns()
        {
            ViewBag.Condicoes = new List<SelectListItem>
            {
                new SelectListItem { Text = "Novo", Value = "1" },
                new SelectListItem { Text = "Usado - Como Novo", Value = "2" },
                new SelectListItem { Text = "Usado - Bom", Value = "3" }
            };

            ViewBag.Categorias = new List<SelectListItem>
            {
                new SelectListItem { Text = "Shounen", Value = "1" },
                new SelectListItem { Text = "Seinen", Value = "2" },
                new SelectListItem { Text = "Shoujo", Value = "3" },
                new SelectListItem { Text = "Isekai", Value = "4" }
            };
        }

        [Route("")]
        public async Task<IActionResult> Index(string termoBusca, string sortOrder, int? pagina)
        {
            ViewData["SortOrderAtual"] = sortOrder;
            ViewData["TituloSort"] = string.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["PrecoSort"] = sortOrder == "preco_asc" ? "preco_desc" : "preco_asc";
            ViewData["EstoqueSort"] = sortOrder == "estoque_asc" ? "estoque_desc" : "estoque_asc";
            ViewData["CondicaoSort"] = sortOrder == "condicao_asc" ? "condicao_desc" : "condicao_asc";
            ViewData["FiltroAtual"] = termoBusca;

            var produtosQuery = _context.Produtos.AsQueryable();

            if (!String.IsNullOrEmpty(termoBusca))
            {
                produtosQuery = produtosQuery.Where(p => p.Titulo.Contains(termoBusca));
            }

            switch (sortOrder)
            {
                case "titulo_desc":
                    produtosQuery = produtosQuery.OrderByDescending(p => p.Titulo);
                    break;
                case "preco_asc":
                    produtosQuery = produtosQuery.OrderBy(p => p.Preco_original);
                    break;
                case "preco_desc":
                    produtosQuery = produtosQuery.OrderByDescending(p => p.Preco_original);
                    break;
                case "estoque_asc":
                    produtosQuery = produtosQuery.OrderBy(p => p.Estoque);
                    break;
                case "estoque_desc":
                    produtosQuery = produtosQuery.OrderByDescending(p => p.Estoque);
                    break;
                case "condicao_asc":
                    produtosQuery = produtosQuery.OrderBy(p => p.Condicao_produto);
                    break;
                case "condicao_desc":
                    produtosQuery = produtosQuery.OrderByDescending(p => p.Condicao_produto);
                    break;
                default:
                    produtosQuery = produtosQuery.OrderBy(p => p.Titulo);
                    break;
            }

            int pageSize = 5;
            int pageNumber = (pagina ?? 1);

            var produtosPaginados = await produtosQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            var totalProdutos = await produtosQuery.CountAsync();

            var viewModel = new ProdutoIndexViewModel
            {
                Produtos = produtosPaginados,
                PaginaAtual = pageNumber,
                TotalPaginas = (int)Math.Ceiling(totalProdutos / (double)pageSize),
                SortOrderAtual = sortOrder,
                FiltroAtual = termoBusca
            };

            return View(viewModel);
        }

        [Route("detalhes/{id:long}")]
        public async Task<IActionResult> Details(long id)
        {
            var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.Produto_ID == id);
            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado!";
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        [Route("adicionar")]
        public IActionResult Create()
        {
            PopularDropdowns();
            return View();
        }

        [HttpPost]
        [Route("adicionar")]
        public async Task<IActionResult> Create(ProdutoCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var novoProduto = new Produto
                {
                    Titulo = viewModel.Titulo,
                    Descricao = viewModel.Descricao,
                    Imagem_url = viewModel.Imagem_url,
                    Estoque = viewModel.Estoque,
                    Condicao_produto = viewModel.Condicao_produto,
                    Altura = viewModel.Altura,
                    Largura = viewModel.Largura,
                    Profundidade = viewModel.Profundidade,
                    FuncionarioId = viewModel.FuncionarioId,
                    Autor = viewModel.Autor,
                    Categoria = viewModel.Categoria,
                    Preco = viewModel.Preco,
                    Preco_original = viewModel.Preco
                };
                try
                {
                    _context.Produtos.Add(novoProduto);
                    await _context.SaveChangesAsync();
                    TempData["MensagemSucesso"] = $"Produto '{novoProduto.Titulo}' cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException as OracleException;
                    string errorMsg = innerException != null ? innerException.Message : ex.Message;
                    TempData["MensagemErro"] = $"Erro ao salvar no banco: {errorMsg}";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro inesperado: {ex.Message}";
                }
            }
            PopularDropdowns();
            return View(viewModel);
        }

        [Route("editar/{id:long}")]
        public async Task<IActionResult> Edit(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);
            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado!";
                return RedirectToAction("Index");
            }

            var viewModel = new ProdutoEditViewModel
            {
                Produto_ID = produto.Produto_ID,
                Titulo = produto.Titulo,
                Descricao = produto.Descricao,
                Imagem_url = produto.Imagem_url,
                Preco = produto.Preco,
                Estoque = produto.Estoque,
                Condicao_produto = produto.Condicao_produto,
                Altura = produto.Altura,
                Largura = produto.Largura,
                Profundidade = produto.Profundidade,
                FuncionarioId = produto.FuncionarioId,
                Autor = produto.Autor,
                Categoria = produto.Categoria

            };

            var condicoes = new List<SelectListItem> {
                new SelectListItem { Text = "Novo", Value = "1" },
                new SelectListItem { Text = "Usado - Como Novo", Value = "2" },
                new SelectListItem { Text = "Usado - Bom", Value = "3" }
            };
            ViewBag.Condicoes = new SelectList(condicoes, "Value", "Text", viewModel.Condicao_produto.ToString());

            var categorias = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Shounen", Value = "1" },
                    new SelectListItem { Text = "Seinen", Value = "2" },
                    new SelectListItem { Text = "Shoujo", Value = "3" },
                    new SelectListItem { Text = "Isekai", Value = "4" }
                };
            ViewBag.Categorias = new SelectList(categorias, "Value", "Text", viewModel.Categoria.ToString());

                return View(viewModel);
        }

        [HttpPost]
        [Route("editar/{id:long?}")]
        public async Task<IActionResult> Edit(ProdutoEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var condicoes = new List<SelectListItem> {
                    new SelectListItem { Text = "Novo", Value = "1" },
                    new SelectListItem { Text = "Usado - Como Novo", Value = "2" },
                    new SelectListItem { Text = "Usado - Bom", Value = "3" }
                };
                ViewBag.Condicoes = new SelectList(condicoes, "Value", "Text", viewModel.Condicao_produto.ToString());
                return View(viewModel);
            }

            var produtoOriginal = await _context.Produtos.FindAsync(viewModel.Produto_ID);

            if (produtoOriginal != null)
            {
                produtoOriginal.Titulo = viewModel.Titulo;
                produtoOriginal.Descricao = viewModel.Descricao;
                produtoOriginal.Imagem_url = viewModel.Imagem_url;
                produtoOriginal.Estoque = viewModel.Estoque;
                produtoOriginal.Condicao_produto = viewModel.Condicao_produto;
                produtoOriginal.Altura = viewModel.Altura;
                produtoOriginal.Largura = viewModel.Largura;
                produtoOriginal.Profundidade = viewModel.Profundidade;
                produtoOriginal.FuncionarioId = viewModel.FuncionarioId;
                produtoOriginal.Autor = viewModel.Autor;
                produtoOriginal.Categoria = viewModel.Categoria;
                produtoOriginal.Preco = viewModel.Preco;

                try
                {
                    _context.Produtos.Update(produtoOriginal);
                    await _context.SaveChangesAsync();
                    TempData["MensagemSucesso"] = $"Produto '{produtoOriginal.Titulo}' editado com sucesso!";
                }
                catch (DbUpdateException ex)
                {
                    var innerException = ex.InnerException as OracleException;
                    string errorMsg = innerException != null ? innerException.Message : ex.Message;
                    TempData["MensagemErro"] = $"Erro ao salvar no banco: {errorMsg}";
                }
                catch (Exception ex)
                {
                    TempData["MensagemErro"] = $"Erro inesperado: {ex.Message}";
                }
            }
            else
            {
                TempData["MensagemErro"] = "Erro ao tentar editar (Produto não encontrado no POST).";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("excluir/{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            var produto = await _context.Produtos.FindAsync(id);

            if (produto == null)
            {
                TempData["MensagemErro"] = "Produto não encontrado!";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Produtos.Remove(produto);
                await _context.SaveChangesAsync();
                TempData["MensagemSucesso"] = $"Produto '{produto.Titulo}' excluído com sucesso!";
            }
            catch (DbUpdateException ex)
            {
                var innerException = ex.InnerException as OracleException;
                if (innerException != null && innerException.Number == 2292)
                {
                    TempData["MensagemErro"] = $"Não é possível excluir o produto '{produto.Titulo}', pois ele já está associado a pedidos ou outros registros.";
                }
                else
                {
                    string errorMsg = innerException != null ? innerException.Message : ex.Message;
                    TempData["MensagemErro"] = $"Erro ao excluir do banco: {errorMsg}";
                }
            }
            catch (Exception ex)
            {
                TempData["MensagemErro"] = $"Erro inesperado: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}