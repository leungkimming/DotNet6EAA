using AutoMapper;
using Common;

namespace Client {
    public class SystemParametersProfile : Profile {
        public SystemParametersProfile() {
            CreateMap<SystemParametersSearchResponse, AddSystemParameterRequest>();
        }
    }
}
