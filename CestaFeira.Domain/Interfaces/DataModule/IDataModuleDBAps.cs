using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.Repository;

namespace CestaFeira.Domain.Interfaces.DataModule
{
    public interface IDataModuleDBAps : IDataModule
    {
        IRepository<ProdutoEntity> ProdutoRepository { get; }
        IRepository<PedidoEntity> PedidoRepository { get; }
        IRepository<UsuarioEntity> UsuarioRepository { get; }

    }
}
