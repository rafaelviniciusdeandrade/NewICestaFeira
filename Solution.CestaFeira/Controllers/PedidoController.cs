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
            // 1. Recupera o carrinho da sessão ou cria um novo se não existir
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho") ?? new List<ItemCarrinhoModel>();

            // 2. Busca o produto (DEVE incluir o FornecedorId)
            var produto = await _produto.ConsultarProdutosId(produtoId);

            if (produto == null)
            {
                return BadRequest(new { success = false, message = "Produto não encontrado" });
            }

            // *******************************************************************
            // 3. VALIDAÇÃO DE FORNECEDOR (Ifood-like)
            // *******************************************************************
            if (carrinho.Any())
            {
                // Obtém o FornecedorId do primeiro item no carrinho
                var fornecedorAtual = carrinho.First().IdUsuario;

                // Compara com o FornecedorId do produto que está sendo adicionado
                if (fornecedorAtual != produto.UsuarioId)
                {
                    // PRODUTO DE OUTRO FORNECEDOR. Retorna a flag para o frontend.
                    return Ok(new
                    {
                        success = false, // Indica que a operação não foi concluída
                        requiresConfirmation = true, // Flag para o frontend
                        message = "Seu carrinho possui itens de outro produtor/fornecedor. Deseja esvaziar o carrinho e adicionar este item?",
                        novoProdutoId = produtoId, // Passa o ID e a quantidade para serem usados após a confirmação
                        novaQuantidade = quantidade
                    });
                }
            }

            // 4. Se chegou aqui, o carrinho está vazio OU o produto é do mesmo fornecedor.

            // Verifica se o produto já está no carrinho
            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);

            // *******************************************************************
            // 5. Verificação de estoque ANTES de adicionar/somar
            // *******************************************************************
            int quantidadeAposAdicao = itemExistente != null ? itemExistente.Quantidade + quantidade : quantidade;

            if (produto.Quantidade < quantidadeAposAdicao)
            {
                return BadRequest(new { success = false, message = "Erro: A quantidade total solicitada é maior que o estoque disponível." });
            }

            if (itemExistente != null)
            {
                // Item já existe: Aumenta a quantidade
                itemExistente.Quantidade = quantidadeAposAdicao; // Usa a quantidade calculada acima
            }
            else
            {
                // Adiciona um novo item no carrinho
                carrinho.Add(new ItemCarrinhoModel
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    // Certifique-se de que produto.FornecedorId esteja sendo populado corretamente
                    IdUsuario = produto.UsuarioId,
                    Quantidade = quantidade,
                    ValorUnitario = produto.valorUnitario
                });
            }

            // 6. Salva o carrinho atualizado na sessão
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            return Ok(new { success = true, message = "Produto adicionado ao carrinho com sucesso!", quantidadeItens = carrinho.Count });
        }

        // *******************************************************************
        // 7. NOVO MÉTODO: Esvaziar e Adicionar (usado após a confirmação do cliente)
        // *******************************************************************

        [HttpPost]
        public async Task<IActionResult> EsvaziarECadicionar(Guid produtoId, int quantidade)
        {
            // Esvazia o carrinho
            HttpContext.Session.Remove("Carrinho");

            // Busca o produto novamente para garantir os dados (e estoque)
            var produto = await _produto.ConsultarProdutosId(produtoId);

            if (produto == null)
            {
                return BadRequest(new { success = false, message = "Produto não encontrado" });
            }

            if (produto.Quantidade < quantidade)
            {
                // Se o estoque for insuficiente mesmo após a confirmação (improvável, mas segurança)
                return BadRequest(new { success = false, message = "Erro: Quantidade maior que o estoque" });
            }

            // Cria um novo carrinho e adiciona o item
            var novoCarrinho = new List<ItemCarrinhoModel>
    {
        new ItemCarrinhoModel
        {
            ProdutoId = produto.Id,
            Nome = produto.Nome,
            IdUsuario = produto.UsuarioId,
            Quantidade = quantidade,
            ValorUnitario = produto.valorUnitario
        }
    };

            // Salva o novo carrinho na sessão
            HttpContext.Session.SetObjectAsJson("Carrinho", novoCarrinho);

            return Ok(new { success = true, message = "Carrinho esvaziado e novo produto adicionado com sucesso!", quantidadeItens = novoCarrinho.Count });
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
