using AutoMapper;
using Business;
using Common;
using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Service {
    public class RequestLogService : BaseService {
        public RequestLogService(IUnitOfWork unitOfWork,
            ILogger<RequestLogService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor) {
        }
        public async void Log(DateTime timeStamp, string route,
                string method,
                string parameters,
                string request,
                string response) {
            var repository = UnitOfWork.AsyncRepository<RequestLog>();
            RequestLog requestLog = new RequestLog();
            requestLog.TimeStamp = timeStamp;
            requestLog.Route = route;
            requestLog.Method = method;
            requestLog.Parameters = parameters;
            requestLog.Request = request;
            requestLog.Response = response;
            requestLog.CreateBy = requestLog.UpdateBy = "System";
            requestLog.CreateTime = requestLog.UpdateTime = DateTime.Now;
            await repository.AddAsync(requestLog);
            await UnitOfWork.SaveChangesAsync();

        }
    }
}
