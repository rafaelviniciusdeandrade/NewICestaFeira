namespace CestaFeira.Web.Services.Interfaces
{
    public interface ICarrinhoService
    {
        void AdicionarProduto(int produtoId);
        int ObterQuantidadeTotal();
    }
}
