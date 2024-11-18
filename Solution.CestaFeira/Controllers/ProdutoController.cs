using CestaFeira.Web.Helpers;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Produto;
using Microsoft.AspNetCore.Mvc;

namespace CestaFeira.Web.Controllers
{
    public class ProdutoController : Controller
    {
        public IProdutoService _produto;
        public IPedidoService _pedido;

        public ProdutoController(IProdutoService produto, IPedidoService pedido)
        {
            _produto = produto;
            _pedido = pedido;
        }

        public async Task<IActionResult> ProdutosAsync()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id= Guid.Parse(usuarioId);
            var result = await _produto.ConsultarTodosProdutos(); 
            return View(result);
       

        }

        public async Task<IActionResult> ProdutosProdutor()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _produto.ConsultarProdutos(id);
            return View(result);


        }

        public async Task<IActionResult> GerenciarProdutos()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _produto.ConsultarProdutos(id);
            return View(result);


        }
        public async Task<IActionResult> EditarProdutos(Guid id)
        {
            var produto = await _produto.ConsultarProdutosId(id);

            if (produto == null)
            {
                return NotFound();
            }

            var produtoModel = new ProdutoModel
            {
                Id = produto.Id,
                Nome = produto.Nome,
                Descricao = produto.Descricao,
                Quantidade = produto.Quantidade,
                valorUnitario = produto.valorUnitario,
                imagem = produto.imagem
            };

            return View(produtoModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditarProdutos(Guid id, ProdutoModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

                var result = await _produto.EditarProduto(model);
                if (result)
                {
                    TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
                    return RedirectToAction("ProdutosProdutor", "Produto");
                }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Excluir(Guid id)
        {
            var sucesso = await _produto.ExcluirProduto(id);

            if (sucesso)
            {
                return Json(new { success = true });
            }
            else
            {
                return Json(new { success = false, message = "Erro ao excluir o produto. Tente novamente." });
            }
        }
        public IActionResult CadastrarProdutos()
        {
            return View("CadastrarProdutos");
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarProdutos(ProdutoModel produtoModel, IFormFile imagemProduto)
        {
            produtoModel.imagem = imagemProduto.ToByteArray();
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            produtoModel.UsuarioId = Guid.Parse(usuarioId);
            var result = await _produto.CadastrarProduto(produtoModel);
            if (result)
            {
                TempData["Sucesso"] = "Produto cadastrado com sucesso!";
                return RedirectToAction("ProdutosProdutor", "Produto");
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possivel Cadastrar o produto";
                return View("CadastrarProdutos", produtoModel);
            }
        }
    }
}
