using Business;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Data {
    public class UserRepository : RepositoryBase<User>
        , IUserRepository {
        private readonly DbSet<User> _dbSet;
        public UserRepository(EFContext dbContext) : base(dbContext) {
            _dbSet = dbContext.Set<User>();
        }
        public Task<List<User>> ListAsyncwithDept(Expression<Func<User, bool>> expression) {
            return _dbSet.Where(expression)
                .Include(User => User.Department)
                .ToListAsync();
        }
        public Task<User?> GetAsyncWithPayslip(Expression<Func<User, bool>> expression) {
            return _dbSet
                .Include(User => User.PaySlips)
                .FirstOrDefaultAsync(expression);
        }
    }
}