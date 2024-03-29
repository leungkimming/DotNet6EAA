﻿using Business;
using System.Linq.Expressions;

namespace Data {
    public interface IUserRepository : IAsyncRepository<User> {
        Task<List<User>> ListAsyncwithDept(Expression<Func<User, bool>> expression);
        Task<User?> GetAsyncWithPayslip(Expression<Func<User, bool>> expression);
        Task<List<User>> ListAsyncwithDeptByPagging(Expression<Func<User, bool>> expression, int pageSize, int pageNo);
    }
}