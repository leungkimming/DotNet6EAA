using Data;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Service {
    public class BaseService {
        public readonly ILogger _logger;
        public readonly IMapper _mapper;
        public readonly HttpContext _httpContext;
        public BaseService(IUnitOfWork unitOfWork,
            ILogger logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) {
            UnitOfWork = unitOfWork;
            this._logger = logger;
            this._mapper = mapper;
            this._httpContext = httpContextAccessor.HttpContext;
        }

        protected internal IUnitOfWork UnitOfWork { get; set; }
    }
}