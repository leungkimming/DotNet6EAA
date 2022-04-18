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

namespace Service {
    public class SystemParametersService : BaseService {
        public SystemParametersService(IUnitOfWork unitOfWork,
            ILogger<UserService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor) {
        }
        public async Task<GetAllDatasResponse<SystemParametersSearchResponse>> SearchAsync(SystemParametersSearchRequest request) {

            var repository = UnitOfWork.SystemParametersRepository();
            GetAllDatasResponse<SystemParametersSearchResponse> getAllDatasResponse=new GetAllDatasResponse<SystemParametersSearchResponse>();
            var systemParameters = await repository
                .ListAsyncByPagging(_ => request.Code==null||_.Code.Contains(request.Code),request.RecordsPerPage,request.PageNo);
            getAllDatasResponse.TotalCount = await repository
               .ListCountAsync(_ => request.Code == null || _.Code.Contains(request.Code));

            getAllDatasResponse.Datas = systemParameters.Select(_parameters => _mapper.Map<SystemParametersSearchResponse>(_parameters)).ToList();
            return getAllDatasResponse;
        }
        public async Task<AddDataResponse> AddNewAsync(AddSystemParameterRequest model) {
            AddDataResponse addDataResponse= new AddDataResponse();
            var repository = UnitOfWork.AsyncRepository<SystemParameters>();
            var checkSystemParameter = await repository.GetAsync(x => x.Code == model.Code);
            if (checkSystemParameter != null) {
                throw new SystemParameterAlreadyExistException(model.Code);
            }

            var systemParameters = _mapper.Map<SystemParameters>(model);
            await repository.AddAsync(systemParameters);
            await UnitOfWork.SaveChangesAsync();
            addDataResponse.IsSuccess = true;
            addDataResponse.Message = "Successfully";
            return addDataResponse;
        }
    }
}
