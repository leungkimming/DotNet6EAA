using Business;
using Common;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Data {
    public class RepositoryBase<T> : IAsyncRepository<T> where T : RootEntity {
        private readonly DbSet<T> _dbSet;

        public RepositoryBase(EFContext dbContext) {
            _dbSet = dbContext.Set<T>();
        }

        public async Task<T> AddAsync(T entity) {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public Task<bool> DeleteAsync(T entity) {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }

        public Task<T> GetAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public Task<List<T>> ListAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.Where(expression).ToListAsync();
        }
        public Task<int> ListCountAsync(Expression<Func<T, bool>> expression) {
            return _dbSet.Where(expression).CountAsync();
        }
        public Task<List<T>> ListAsyncByPagging(Expression<Func<T, bool>> expression, int pageSize, int pageNo) {
            return _dbSet.Where(expression).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public IQueryable<T> ListAsyncByPaggingQueryable(Expression<Func<T, bool>> expression, int pageSize, int pageNo) {
            return _dbSet.Where(expression).Skip((pageNo - 1) * pageSize).Take(pageSize);
        }

        public Task<T> UpdateAsync(T entity) {
            _dbSet.Update(entity);
            return Task.FromResult(entity);
        }

        public Task<T> UpdateWithPreValidationAsync(DTObase updateRequest, T entity) {
            PreValidation(updateRequest, entity);
            return UpdateAsync(entity);
        }
        public virtual void PreValidation(DTObase updateRequest, T entity) {
            if (entity is IRowVersionContract updateEntity && updateRequest.RowVersion != null && !updateEntity.RowVersion.SequenceEqual(updateRequest.RowVersion)) {
                throw new Exception("Record has already been updated or deleted by another user.");
            }

        }
    }
}