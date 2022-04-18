using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data {
    public class SystemParametersRepository : RepositoryBase<SystemParameters>
        , ISystemParametersRepository {
        private readonly DbSet<SystemParameters> _dbSet;
        public SystemParametersRepository(EFContext dbContext) : base(dbContext) {
            _dbSet = dbContext.Set<SystemParameters>();
        }

    }
}
