using CestaFeira.Web.Helpers.Session;
using CestaFeira.Web.Models.Carrinho;
using CestaFeira.Web.Models.Produto;
using CestaFeira.Web.Services.Interfaces;
using CestaFeira.Web.Services.Produto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System.Text.Json;

namespace CestaFeira.Web.Controllers
{
    public class CarrinhoController : Controller
    {
        public IProdutoService _produto;
        private readonly ICarrinhoService _carrinhoService;

        public CarrinhoController(ICarrinhoService carrinhoService, IProdutoService produto)
        {
            _carrinhoService = carrinhoService;
            _produto = produto;
        }

        //[HttpPost]
        //public IActionResult AdicionarAoCarrinho(int produtoId)
        //{
        //    _carrinhoService.AdicionarProduto(produtoId);
        //    int novaQuantidade = ObterQuantidadeAtualDoCarrinho(); // Sua lógica para obter a quantidade total

        //    return Json(new { quantidade = novaQuantidade }); // Retorna a nova quantidade
        //    //return RedirectToAction("Produtos", "Produto"); // redirecionar de volta para a página principal
        //}
        public IActionResult ObterQuantidadeCarrinho()
        {
            var quantidade = _carrinhoService.ObterQuantidadeTotal();
            return Json(new { quantidade });
        }

        //[HttpPost]
        //public async Task<IActionResult> AdicionarProdutoAoCarrinho(Guid produtoId)
        //{
        //    var carrinhoJson = HttpContext.Session.GetString("Carrinho");
        //    var carrinho = string.IsNullOrEmpty(carrinhoJson)
        //        ? new List<ProdutoModel>()
        //        : JsonSerializer.Deserialize<List<ProdutoModel>>(carrinhoJson);

        //    // Consultar produto pelo ID
        //    var produto = await _produto.ConsultarProdutosId(produtoId);

        //    if (produto != null)
        //    {
        //        carrinho.Add(produto);
        //        // Armazena a lista de produtos novamente na sessão como JSON
        //        HttpContext.Session.SetString("Carrinho", JsonSerializer.Serialize(carrinho));
        //    }

        //    // Retorna a quantidade de itens no carrinho
        //    return Json(new { success = true, quantidadeItens = carrinho.Count });
        //}


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
        public async Task<IActionResult> AdicionarProdutoAoCarrinho(Guid produtoId)
        {
            // Recupera o carrinho da sessão ou cria um novo se não existir
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinhoModel>>("Carrinho") ?? new List<ItemCarrinhoModel>();

            // Busca o produto (simulação, você pode recuperar da sua lista de produtos)
            var produto = await _produto.ConsultarProdutosId(produtoId);

            if (produto == null)
            {
                return BadRequest(new { success = false, message = "Produto não encontrado" });
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
                    Quantidade = 1,
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
                return Ok(new { success = false, message = "O carrinho está vazio." });
            }

            // Log para verificar o conteúdo do carrinho
            System.Diagnostics.Debug.WriteLine("Itens no carrinho: " + Newtonsoft.Json.JsonConvert.SerializeObject(carrinho));

            return Ok(new { success = true, itens = carrinho });
        }


        private int ObterQuantidadeAtualDoCarrinho()
        {
            var carrinhoJson = HttpContext.Session.GetString("Carrinho");
            var carrinho = string.IsNullOrEmpty(carrinhoJson)
                ? new List<ProdutoModel>()
                : JsonSerializer.Deserialize<List<ProdutoModel>>(carrinhoJson);

            return carrinho.Count; // Retorna a quantidade de produtos no carrinho
        }
        public static Guid ConvertIntToGuid(int number)
        {
            // Converte o int para um array de bytes de 4 bytes
            byte[] intBytes = BitConverter.GetBytes(number);

            // Cria um novo Guid a partir dos bytes
            // Usando Guid.NewGuid() para preencher os restantes dos bytes
            byte[] guidBytes = new byte[16];
            Array.Copy(intBytes, guidBytes, intBytes.Length);
            Guid newGuid = new Guid(guidBytes);

            return newGuid;
        }




    }
}
