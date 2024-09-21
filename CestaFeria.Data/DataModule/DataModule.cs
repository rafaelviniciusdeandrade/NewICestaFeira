using CestaFeira.Domain.Interfaces.DataModule;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CestaFeria.Data.DataModule
{
    public class DataModule<TDbContext> : IDataModule, IDisposable where TDbContext : DbContext
    {
        public DataModule(TDbContext dbContext)
        {
            CurrentContext = dbContext;
        }

        public readonly TDbContext CurrentContext;

        public IDbContextTransaction CurrentTransaction { get; set; }

        private bool ActiveTransaction { get; set; } = false;

        private bool _disposed = false;

        public async Task StartTransactionAsync()
        {
            CurrentTransaction = await CurrentContext.Database.BeginTransactionAsync();
            ActiveTransaction = true;
        }

        public async Task CommitDataAsync()
        {
            await CurrentContext.SaveChangesAsync();
            if (ActiveTransaction)
                await CommitTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (ActiveTransaction)
                await CurrentContext.Database.CommitTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            if (ActiveTransaction)
                await CurrentContext.Database.RollbackTransactionAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed && disposing)
            {
                CurrentContext.Dispose();
            }
            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
