using CestaFeira.Domain.Interfaces.Repository;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CestaFeria.Data.Context;
using CestaFeira.Domain.Entityes.Base;

namespace CestaFeria.Data.Repository
{
    public class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApsContext _context;
        private readonly DbSet<T> _dataset;

        public BaseRepository(ApsContext context)
        {
            _context = context;
            _dataset = _context.Set<T>();
        }

        public DbSet<T> DataSet { get => _dataset; }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
                if (result == null)
                    return false;

                _dataset.Remove(result);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> InsertAsync(T entity)
        {
            try
            {
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                await _dataset.AddAsync(entity);

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public async Task<bool> IsExists(Guid id)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
                return (result != null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> SelectAsync(Guid id)
        {
            try
            {
                return await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(id));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> SelectAsync()
        {
            try
            {
                return await _dataset.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<T> UpdateAsync(T entity)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(entity.Id));
                if (result == null)
                    return null;

                _context.Entry(result).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public async Task<T> AtivarDesativarAsync(T entity)
        {
            try
            {
                var result = await _dataset.SingleOrDefaultAsync(p => p.Id.Equals(entity.Id));
                if (result == null)
                    return null;

                _context.Entry(result).CurrentValues.SetValues(entity);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            return entity;
        }

        public virtual IQueryable<T> ListNoTracking(Expression<Func<T, bool>> predicate) =>
            _dataset.Where(predicate).AsNoTracking();

        public virtual async Task<T> FirstOrDefaultNoTrackingAsync(Expression<Func<T, bool>> predicate) =>
            await _dataset.AsNoTracking().FirstOrDefaultAsync(predicate);

        //public List<ListaEntity> GetById(Func<ListaEntity, bool> predicate)
        //{
        //    return _context.Listas
        //        .Include(l => l.ListaTarefas)
        //        .AsNoTracking()
        //        .Where(predicate)
        //        .ToList();
        //}
        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }

    }
}
