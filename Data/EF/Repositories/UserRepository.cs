﻿using Business.Users;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Data.EF.Interfaces;

namespace Data.EF.Repositories
{
    public class UserRepository : RepositoryBase<User>
        , IUserRepository
    {
        private readonly DbSet<User> _dbSet;
        public UserRepository(EFContext dbContext) : base(dbContext)
        {
            _dbSet = dbContext.Set<User>();
        }
        public Task<List<User>> ListAsyncwithDept(Expression<Func<User, bool>> expression)
        {
            return _dbSet.Where(expression)
                .Include(User => User.Department)
                .ToListAsync();
        }
    }
}