using CestaFeira.Web.Helpers.Session;
using CestaFeira.Web.Models.Carrinho;
using CestaFeira.Web.Models.Pedido;
using CestaFeira.Web.Models.Pix;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Email;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Pedido;
using CestaFeira.Web.Services.Pix;
using CestaFeira.Web.Services.Produto;
using CestaFeira.Web.Services.Usuario;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CestaFeira.Web.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IProdutoService _produto;
        private readonly IPedidoService _pedido;
        private readonly IUsuarioService _usuario;
        private readonly IPedidoService _carrinhoService;
        private readonly IPixService _pixService;
        private readonly IEmailService _emailService;

        public PedidoController(
            IPedidoService carrinhoService,
            IProdutoService produto,
            IPedidoService pedido,
            IPixService pixService,
            IEmailService emailService,
            IUsuarioService usuario)
        {
            _carrinhoService = carrinhoService;
            _produto = produto;
            _pedido = pedido;
            _pixService = pixService;
            _emailService = emailService;
            _usuario = usuario;
        }

        public IActionResult ObterQuantidadeCarrinho()
        {
            var quantidade = _carrinhoService.ObterQuantidadeTotal();
            return Json(new { quantidade });
        }

        [HttpGet]
        public IActionResult ObterQuantidadeItensCarrinho()
        {
            // Carrinho é salvo como List<ItemCarrinhoModel>, não ProdutoModel
            var carrinho = HttpContext.Session
                .GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho")
                ?? new List<ItemCarrinhoModel>();

            int quantidadeTotal = carrinho.Sum(p => p.Quantidade);

            return Json(new { success = true, quantidadeItens = quantidadeTotal });
        }

        [HttpPost]
        public async Task<IActionResult> AdicionarProdutoAoCarrinho(Guid produtoId, int quantidade)
        {
            var carrinho = HttpContext.Session
                .GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho")
                ?? new List<ItemCarrinhoModel>();

            var produto = await _produto.ConsultarProdutosId(produtoId);
            if (produto == null)
                return BadRequest(new { success = false, message = "Produto não encontrado" });

            // valida produtor único no carrinho
            if (carrinho.Any())
            {
                var fornecedorAtual = carrinho.First().IdUsuario;
                if (fornecedorAtual != produto.UsuarioId)
                {
                    return Ok(new
                    {
                        success = false,
                        requiresConfirmation = true,
                        message = "Seu carrinho possui itens de outro produtor/fornecedor. Deseja esvaziar o carrinho e adicionar este item?",
                        novoProdutoId = produtoId,
                        novaQuantidade = quantidade
                    });
                }
            }

            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == produtoId);
            int quantidadeAposAdicao = itemExistente != null
                ? itemExistente.Quantidade + quantidade
                : quantidade;

            if (produto.Quantidade < quantidadeAposAdicao)
                return BadRequest(new { success = false, message = "Erro: A quantidade total solicitada é maior que o estoque disponível." });

            if (itemExistente != null)
            {
                itemExistente.Quantidade = quantidadeAposAdicao;
            }
            else
            {
                carrinho.Add(new ItemCarrinhoModel
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    IdUsuario = (Guid)produto.UsuarioId,
                    Quantidade = quantidade,
                    ValorUnitario = produto.valorUnitario
                });
            }

            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            return Ok(new
            {
                success = true,
                message = "Produto adicionado ao carrinho com sucesso!",
                quantidadeItens = carrinho.Sum(c => c.Quantidade)
            });
        }

        [HttpPost]
        public async Task<IActionResult> EsvaziarECadicionar(Guid produtoId, int quantidade)
        {
            HttpContext.Session.Remove("Carrinho");

            var produto = await _produto.ConsultarProdutosId(produtoId);
            if (produto == null)
                return BadRequest(new { success = false, message = "Produto não encontrado" });

            if (produto.Quantidade < quantidade)
                return BadRequest(new { success = false, message = "Erro: Quantidade maior que o estoque" });

            var novoCarrinho = new List<ItemCarrinhoModel>
            {
                new ItemCarrinhoModel
                {
                    ProdutoId = produto.Id,
                    Nome = produto.Nome,
                    IdUsuario = (Guid)produto.UsuarioId,
                    Quantidade = quantidade,
                    ValorUnitario = produto.valorUnitario
                }
            };

            HttpContext.Session.SetObjectAsJson("Carrinho", novoCarrinho);

            return Ok(new
            {
                success = true,
                message = "Carrinho esvaziado e novo produto adicionado com sucesso!",
                quantidadeItens = novoCarrinho.Sum(c => c.Quantidade)
            });
        }

        [HttpGet]
        public IActionResult ObterItensCarrinho()
        {
            var carrinho = HttpContext.Session
                .GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho")
                ?? new List<ItemCarrinhoModel>();

            if (!carrinho.Any())
                return Json(new { success = true, itens = new List<object>(), total = 0 });

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
            if (pedidoViewModel == null || pedidoViewModel.Produtos == null || !pedidoViewModel.Produtos.Any())
                return BadRequest("Carrinho vazio");

            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            pedidoViewModel.UsuarioId = Guid.Parse(usuarioId);
            pedidoViewModel.Data = DateTime.Now;

            var result = await _carrinhoService.CadastrarCarrinho(pedidoViewModel);

            if (!result)
            {
                TempData["ErrorMessage"] = "Não foi possível salvar o pedido.";
                return RedirectToAction("Produtos", "Produto");
            }

            // limpa carrinho
            HttpContext.Session.Remove("Carrinho");

            try
            {
                var produtor = await _usuario.ConsultarUsuario((Guid)pedidoViewModel.Produtos.First().UsuarioId);

                if (produtor != null)
                {
                    string emailProdutor = produtor.Email;
                    string assunto = $"Novo Pedido Recebido";
                    string corpo = $"Olá {produtor.Nome},<br><br>" +
                                   $"Você recebeu um novo pedido (**#{pedidoViewModel.UsuarioId}**)!<br>" +
                                   $"Data: {pedidoViewModel.Data:dd/MM/yyyy HH:mm}<br>" +
                                   $"Total de Itens: {pedidoViewModel.Produtos.Count()}<br>" +
                                   "Por favor, acesse o painel para visualizar os detalhes e processar o pedido.<br><br>" +
                                   "Atenciosamente,<br>CestaFeira.";

                    await _emailService.SendEmailAsync(emailProdutor, assunto, corpo);
                }

                TempData["Sucesso"] = "Pedido cadastrado com sucesso!";
                return RedirectToAction("Produtos", "Produto");
            }
            catch
            {
                TempData["Sucesso"] = "Pedido cadastrado com sucesso!";
                return RedirectToAction("Produtos", "Produto");
            }
        }

        [HttpPost]
        public async Task<IActionResult> GerarPix([FromBody] PixRequest request)
        {
            try
            {
                var carrinho = HttpContext.Session
                    .GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho")
                    ?? new List<ItemCarrinhoModel>();

                if (!carrinho.Any())
                    return Json(new PixResponse { Success = false, Message = "Carrinho vazio. Não é possível gerar Pix." });

                // total vindo da tela (mais correto que pegar um item só)
                decimal valorTotal =
     request.ValorTotal > 0
     ? request.ValorTotal
     : carrinho.Sum(c => (decimal)c.Quantidade * (decimal)c.ValorUnitario);


                if (valorTotal <= 0)
                    return Json(new PixResponse { Success = false, Message = "Valor total inválido para geração do Pix." });

                var produtorId = carrinho.First().IdUsuario;
                var produtor = await _usuario.ConsultarUsuario(produtorId);

                if (produtor == null)
                    return Json(new PixResponse { Success = false, Message = "Produtor não encontrado." });

                string produtorCpf = produtor.cpf; // ajuste se o campo tiver outro nome
                if (string.IsNullOrWhiteSpace(produtorCpf))
                    return Json(new PixResponse { Success = false, Message = "CPF/Chave Pix do produtor não está configurado." });

                var pixResult = await _pixService.GerarQrCodePix(valorTotal, produtorCpf, produtor.Nome);

                if (!pixResult.Success)
                    return Json(pixResult);

                return Json(pixResult);
            }
            catch (Exception ex)
            {
                return Json(new PixResponse
                {
                    Success = false,
                    Message = $"Erro interno do servidor: {ex.Message}"
                });
            }
        }

        public async Task<IActionResult> Pedidos()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _pedido.ConsultarPedidos(id);
            return View(result);
        }

        public async Task<IActionResult> GerenciarPedidos()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _pedido.ConsultarPedidosProdutor(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> CancelarPedido(Guid pedidoId)
        {
            var status = "Cancelado";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("Pedidos");
            else
            {
                TempData["Erro"] = "Erro ao cancelar.";
                return RedirectToAction("Pedidos");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AceitarPedido(Guid pedidoId)
        {
            var status = "Em andamento";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("GerenciarPedidos");
            else
            {
                TempData["Erro"] = "Erro ao aceitar o pedido.";
                return RedirectToAction("GerenciarPedidos");
            }
        }

        [HttpPost]
        public async Task<IActionResult> RecusarPedido(Guid pedidoId)
        {
            var status = "Cancelado";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("GerenciarPedidos");
            else
            {
                TempData["Erro"] = "Erro ao recusar o pedido.";
                return RedirectToAction("GerenciarPedidos");
            }
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarPedido(Guid pedidoId)
        {
            var status = "Finalizado";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("GerenciarPedidos");
            else
            {
                TempData["Erro"] = "Erro ao finalizar o pedido.";
                return RedirectToAction("GerenciarPedidos");
            }
        }

        /// <summary>
        /// Action chamada pelo cliente quando o pedido já está "Em andamento"
        /// e ele confirma que recebeu o produto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ConfirmarEntrega(Guid pedidoId)
        {
            var status = "Entregue";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("Pedidos");
            else
            {
                TempData["Erro"] = "Erro ao confirmar entrega.";
                return RedirectToAction("Pedidos");
            }
        }
    }
}
