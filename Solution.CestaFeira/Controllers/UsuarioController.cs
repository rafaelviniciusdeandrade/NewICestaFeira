using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using CestaFeira.Web.Models.Usuario;
using CestaFeira.Web.Services.Interfaces;
using Nest;

namespace CestaFeira.Web.Controllers
{
    public class UsuarioController : Controller
    {
        public IUsuarioService _usuario;

        public UsuarioController(IUsuarioService usuario)
        {
            _usuario = usuario;
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
                var ret=await _usuario.ValidarUsuario(model);
                if (ret != null) {
                    HttpContext.Session.SetString("UsuarioId", ret.Id.ToString());
                    ClaimsIdentity? identity = null;
                    identity = new ClaimsIdentity(new[]
                   {
                        new Claim(ClaimTypes.Email, ret.Email.ToString()),
                        new Claim(ClaimTypes.Role, ret.Perfil)
                    }, CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (ret.Perfil == "ADM")
                    {
                        return RedirectToAction("Produtos", "Produto");
                    }
                    if (ret.Perfil == "COMUM")
                    {
                        return RedirectToAction("Produtos", "Produto");
                    }
                    if (ret.Perfil == "PROD")
                    {
                        return RedirectToAction("ProdutosProdutor", "Produto");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "";
                        return View("Login", model);
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Usuario ou senha incorretos";
                    return View("Login", model);
                }



            }
            else
            {
                TempData["ErrorMessage"] = "";
                return View("Login", model);
            }
        }

        public IActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario(UsuarioModel usuario)
        {
            try {
                if (usuario.NomeFantasia == null)
                    usuario.NomeFantasia = "";
                    var ret = await _usuario.CadastrarUsuario(usuario);
                    
                    if (ret)
                    {
                    TempData["SucessMessage"] = "Usuário Cadastrado com sucesso";
                    
                        return RedirectToAction("Login", "Usuario"); // Redireciona para a tela de login após o cadastro

                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "Usuario não cadastrado" });

                    }
            }
            catch (Exception ex)
            {
                string mensagemCompleta = ex.Message;
                string mensagemExtraida = mensagemCompleta.Split(':')[2].Split("Severity")[0].Trim();

                TempData["ErrorMessage"] = mensagemExtraida;

                return RedirectToAction("CadastrarUsuario", "Usuario");
            }

            return View(usuario);
        }

        [HttpGet]
        public async Task<IActionResult> Sair()
        {
            // Remove todos os dados da sessão
            HttpContext.Session.Clear();

            // Remove os cookies e faz logout do usuário
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redireciona para a página de login (ou outra página de sua escolha)
            return RedirectToAction("Login", "Usuario");
        }
    }

}


