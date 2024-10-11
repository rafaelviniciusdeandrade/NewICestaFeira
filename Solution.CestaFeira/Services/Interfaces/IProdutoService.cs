using CestaFeira.Web.Models.Produto;

namespace CestaFeira.Web.Services.Interfaces
{
    public interface IProdutoService
    {
        Task<bool> CadastrarProduto(ProdutoModel login);
        Task<List<ProdutoModel>> ConsultarProdutos(Guid UsuarioId);
        Task<ProdutoModel> ConsultarProdutosId(Guid ProdutoId);
    }
}
