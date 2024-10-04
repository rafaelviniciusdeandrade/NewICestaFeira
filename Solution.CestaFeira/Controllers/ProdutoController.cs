using Microsoft.AspNetCore.Mvc;

namespace CestaFeira.Web.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Produtos()
        {
            return View();
        }
        public IActionResult CadastrarProdutos()
        {
            return View();
        }
    }
}
