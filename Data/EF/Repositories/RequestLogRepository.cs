using Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data {
    public class RequestLogRepository : RepositoryBase<RequestLog>
        , IRequestLogRepository {
        private readonly DbSet<RequestLog> _dbSet;
        public RequestLogRepository(EFContext dbContext) : base(dbContext) {
            _dbSet = dbContext.Set<RequestLog>();
        }

    }
}
