using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CestaFeira.Web.Models.Usuario;

namespace CestaFeira.Web.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UsuarioLoginModel model)
        {
            if (ModelState.IsValid)
            {

                return RedirectToAction("CarteiraVacinacao", "Pet");

            }
            else
            {
                TempData["ErrorMessage"] = "";
                return View("Login", model);
            }
        }

    }
}

