using CestaFeira.Web.Helpers.Session;
using CestaFeira.Web.Models.Carrinho;
using CestaFeira.Web.Models.Pedido;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Pedido;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace CestaFeira.Web.Controllers
{
    public class PedidoController : Controller
    {
        public IProdutoService _produto;
        public IPedidoService _pedido;
        private readonly IPedidoService _carrinhoService;

        public PedidoController(IPedidoService carrinhoService, IProdutoService produto, IPedidoService pedido)
        {
            _carrinhoService = carrinhoService;
            _produto = produto;
            _pedido = pedido;
        }
        public IActionResult ObterQuantidadeCarrinho()
        {
            var quantidade = _carrinhoService.ObterQuantidadeTotal();
            return Json(new { quantidade });
        }

        [HttpGet]
        public IActionResult ObterQuantidadeItensCarrinho()
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");
            var carrinho = string.IsNullOrEmpty(carrinhoJson)
                ? new List<ProdutoModel>()
                : JsonSerializer.Deserialize<List<ProdutoModel>>(carrinhoJson);

            return Json(new { sucesso = true, quantidadeItens = carrinho.Count });
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarProdutoAoCarrinho(Guid produtoId, int quantidade)
        {
            // Recupera o carrinho da sessão ou cria um novo se não existir
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho") ?? new List<ItemCarrinhoModel>();

            // Busca o produto (simulação, você pode recuperar da sua lista de produtos)
            var produto = await _produto.ConsultarProdutosId(produtoId);

            if (produto == null)
            {
                return BadRequest(new { success = false, message = "Produto não encontrado" });
            }

            if (produto.Quantidade<quantidade)
            {
                return BadRequest(new { success = false, message = "Erro: Quantidade maior que o estoque" });

            }

            // Verifica se o produto já está no carrinho
            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
            if (itemExistente != null)
            {
                itemExistente.Quantidade++;  // Aumenta a quantidade se já existe no carrinho
            }
            else
            {
                // Adiciona um novo item no carrinho
                carrinho.Add(new ItemCarrinhoModel
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    Quantidade = quantidade,
                    ValorUnitario = produto.valorUnitario
                });
            }

            // Salva o carrinho atualizado na sessão
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            return Ok(new { success = true, message = "Produto adicionado ao carrinho com sucesso!", quantidadeItens = carrinho.Count });
        }

        [HttpGet]
        public IActionResult ObterItensCarrinho()
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho") ?? new List<ItemCarrinhoModel>();

            if (!carrinho.Any())
            {
                return Json(new { success = true, itens = new List<object>(), total = 0 });
            }
            var valorTotal = carrinho.Sum(item => item.Quantidade * item.ValorUnitario);

            return Json(new
            {
                success = true,
                itens = carrinho.Select(item => new
                {
                    item.Nome,
                    item.Quantidade,
                    item.ValorUnitario,
                    item.ProdutoId
                }).ToList(),
                total = valorTotal
            });
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarCompra([FromBody] PedidoModel pedidoViewModel)
        {
            if (pedidoViewModel == null || !pedidoViewModel.Produtos.Any())
            {
                return BadRequest("Carrinho vazio");
            }
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            pedidoViewModel.UsuarioId = Guid.Parse(usuarioId);
            pedidoViewModel.Data = DateTime.Now;
            var result = await _carrinhoService.CadastrarCarrinho(pedidoViewModel);
            if (result)
            {
                TempData["Sucesso"] = "Pedido cadastrado com sucesso!";
                return RedirectToAction("Produtos", "Produto");
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi salvar o pedido";
                return RedirectToAction("Produtos", "Produto");
            }

            
        }
        public async Task<IActionResult> Pedidos()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _pedido.ConsultarPedidos(id);
            return View(result);

        }

        public async Task<IActionResult> PedidosProdutor()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _pedido.ConsultarPedidos(id);
            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> CancelarPedido(Guid pedidoId)
        {
            var status = "Cancelado";
            // Busca o pedido pelo ID
            var pedido = await _pedido.AtualizarStatusPedido(pedidoId,status);
            if (pedido)
            {
                return RedirectToAction("Pedidos");
            }
            else
            {
                TempData["Erro"] = "Erro ao cancelar.";
                return RedirectToAction("Pedidos");
            }

          
        }

    }
}
