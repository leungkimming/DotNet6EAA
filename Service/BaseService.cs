using Data.EF.Interfaces;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace Service
{
    public class BaseService
    {
        public readonly ILogger _logger;
        public readonly IMapper _mapper;
        public BaseService(IUnitOfWork unitOfWork, ILogger logger, IMapper mapper)
        {
            UnitOfWork = unitOfWork;
            this._logger = logger;
            this._mapper = mapper;
        }

        protected internal IUnitOfWork UnitOfWork { get; set; }
    }
}