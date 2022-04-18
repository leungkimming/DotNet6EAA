using AutoMapper;
using Common;
using Business;

namespace Service {
    public class SystemParametersProfile : Profile {
        public SystemParametersProfile() {
            CreateMap<SystemParameters, SystemParametersSearchResponse>();
            CreateMap<AddSystemParameterRequest, SystemParameters>();
            CreateMap<SystemParametersSearchResponse, AddSystemParameterRequest>();
        }
    }
}
