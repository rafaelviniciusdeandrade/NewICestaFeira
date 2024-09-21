using CestaFeira.Domain.Entityes;
using CestaFeira.Domain.Interfaces.DataModule;
using CestaFeira.Domain.Interfaces.Repository;
using CestaFeria.Data.Context;
using CestaFeria.Data.Repository;

namespace CestaFeria.Data.DataModule
{
    public class DataModuleDBAps : DataModule<ApsContext>, IDataModuleDBAps
    {
        public DataModuleDBAps(ApsContext dbContext)
        : base(dbContext) { }


        public IRepository<VendaEntity> _vendaRepository = null;
        public IRepository<VendaEntity> VendaRepository
        {
            get => _vendaRepository ??= new BaseRepository<VendaEntity>(CurrentContext);
        }

        public IRepository<ProdutoEntity> _produtoTarefasRepository = null;
        public IRepository<ProdutoEntity> ProdutoRepository
        {
            get => _produtoTarefasRepository ??= new BaseRepository<ProdutoEntity>(CurrentContext);
        }

        public IRepository<UsuarioEntity> _usuarioRepository = null;
        public IRepository<UsuarioEntity> UsuarioRepository
        {
            get => _usuarioRepository ??= new BaseRepository<UsuarioEntity>(CurrentContext);
        }
    }
}