using Microsoft.EntityFrameworkCore.Storage;

namespace CestaFeira.Domain.Interfaces.DataModule
{
    public interface IDataModule
    {
        IDbContextTransaction CurrentTransaction { get; set; }
        Task StartTransactionAsync();
        Task CommitDataAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}