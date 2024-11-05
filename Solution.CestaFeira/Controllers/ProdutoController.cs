using CestaFeira.Web.Helpers;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CestaFeira.Web.Controllers
{
    public class ProdutoController : Controller
    {
        public IProdutoService _produto;
        public ProdutoController(IProdutoService produto)
        {
            _produto = produto;
        }

        public async Task<IActionResult> ProdutosAsync()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id= Guid.Parse(usuarioId);
            var result = await _produto.ConsultarProdutos(id); 
            return View(result);
       

        }

        public async Task<IActionResult> ProdutosProdutor()
        {
            string usuarioId = HttpContext.Session.GetString("UsuarioId");
            Guid id = Guid.Parse(usuarioId);
            var result = await _produto.ConsultarProdutos(id);
            return View(result);


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
                TempData["Sucesso"] = "Usuário cadastrado com sucesso!";
                return RedirectToAction("Produtos", "Produto");
            }
            else
            {
                TempData["ErrorMessage"] = "Não foi possivel Cadastrar o produto";
                return View("CadastrarProdutos", produtoModel);
            }
        }
    }
}
