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
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace CestaFeira.Web.Controllers
{
    public class PedidoController : Controller
    {
        public IProdutoService _produto;
        public IPedidoService _pedido;
        public IUsuarioService _usuario;
        private readonly IPedidoService _carrinhoService;
        private IPixService _pixService;
        private readonly IEmailService _emailService;

        public PedidoController(IPedidoService carrinhoService, IProdutoService produto, IPedidoService pedido, IPixService pixService, IEmailService emailService, IUsuarioService usuario)
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
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");
            var carrinho = string.IsNullOrEmpty(carrinhoJson)
                ? new List<ProdutoModel>()
                : JsonSerializer.Deserialize<List<ProdutoModel>>(carrinhoJson);

            // Soma REAL das quantidades
            int quantidadeTotal = carrinho.Sum(p => p.Quantidade);

            return Json(new { success = true, quantidadeItens = quantidadeTotal });
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
                    IdUsuario = (Guid)produto.UsuarioId,
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
            IdUsuario = (Guid)produto.UsuarioId,
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
                // ✅ LIMPA O CARRINHO NA SESSÃO
                HttpContext.Session.Remove("Carrinho");

                try
                {
                    var produtor = await _usuario.ConsultarUsuario((Guid)pedidoViewModel.Produtos.First().UsuarioId);

                    if (produtor != null)
                    {
                        string emailProdutor = produtor.Email;
                        string assunto = $"Novo Pedido Recebido: ";
                        string corpo = $"Olá {produtor.Nome},<br><br>" +
                                       $"Você recebeu um novo pedido (**#{pedidoViewModel.UsuarioId}**)!<br>" +
                                       $"Data: {pedidoViewModel.Data:dd/MM/yyyy HH:mm}<br>" +
                                       $"Total de Itens: {pedidoViewModel.Produtos.Count()}<br>" +
                                       "Por favor, acesse o painel para visualizar os detalhes e processar o pedido.<br><br>" +
                                       "Atenciosamente,<br>Sua Equipe de Vendas.";

                        await _emailService.SendEmailAsync(emailProdutor, assunto, corpo);
                        TempData["Sucesso"] = "Pedido cadastrado com sucesso!";
                        return RedirectToAction("Produtos", "Produto");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Não foi salvar o pedido";
                        return RedirectToAction("Produtos", "Produto");
                    }
                }
                catch
                {
                    TempData["Sucesso"] = "Pedido cadastrado com sucesso!";
                    return RedirectToAction("Produtos", "Produto");
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi salvar o pedido";
                return RedirectToAction("Produtos", "Produto");
            }
        }



        [HttpPost]
        public async Task<IActionResult> GerarPix([FromBody] PixRequest request)
        {


            try
            {


                // 1) Recupera o carrinho da sessão
                var carrinho = HttpContext.Session
                    .GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho")
                    ?? new List<ItemCarrinhoModel>();

                if (!carrinho.Any())
                {
                    return Json(new PixResponse
                    {
                        Success = false,
                        Message = "Carrinho vazio. Não é possível gerar Pix."
                    });
                }

                // 2) Pega o produtorId (Guid) do primeiro item
                var produtorId = carrinho.First().IdUsuario;   // GUID

                // 3) Consulta o produtor
                // Se seu método aceita Guid:
                var produtor = await _usuario.ConsultarUsuario(produtorId);

                // Se seu método aceita string, use:
                // var produtor = await _usuario.ConsultarUsuario(produtorId.ToString());

                if (produtor == null)
                {
                    return Json(new PixResponse
                    {
                        Success = false,
                        Message = "Produtor não encontrado."
                    });
                }

                // 4) Pega o CPF / Chave Pix do produtor
                string produtorCpf = produtor.cpf; // ajuste para CpfCnpj/ChavePix se for o caso

                if (string.IsNullOrWhiteSpace(produtorCpf))
                {
                    return Json(new PixResponse
                    {
                        Success = false,
                        Message = "CPF/Chave Pix do produtor não está configurado."
                    });
                }
                // 2. CHAMADA AO SERVIÇO (Ponto de interrupção aqui)
                var pixResult = await _pixService.GerarQrCodePix(Convert.ToDecimal(carrinho.First().ValorUnitario), produtorCpf,produtor.Nome);

                // 3. O SERVIÇO RETORNOU SUCESSO? (Ponto de interrupção aqui)
                if (!pixResult.Success)
                {
                    // Se cair aqui, o erro foi dentro do PixService.
                    return Json(pixResult);
                }

                return Json(pixResult);
            }
            catch (Exception ex)
            {
                // 4. ERRO CRÍTICO NÃO TRATADO (Ponto de interrupção aqui)
                // Se cair aqui, o QRCoder falhou ou há um erro de serialização.
                // Verifique o ex.Message!
                return Json(new PixResponse { Success = false, Message = $"Erro interno do servidor: {ex.Message}" });
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
            // Busca o pedido pelo ID
            var pedido = await _pedido.AtualizarStatusPedido(pedidoId, status);
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
        [HttpPost]
        public async Task<IActionResult> AceitarPedido(Guid pedidoId)
        {
            var status = "Em andamento";
            var ok = await _pedido.AtualizarStatusPedido(pedidoId, status);

            if (ok)
                return RedirectToAction("GerenciarPedidos"); // tela do produtor
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


    }
}
