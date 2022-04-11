using AutoMapper;
using Common;
using Business;

namespace Service {
    public class UserProfile : Profile {
        public UserProfile() {
            CreateMap<AddUserRequest, User>();
            CreateMap<User, AddUserResponse>();
            CreateMap<User, UserInfoDTO>();
            CreateMap<Payslip, PayslipDTO>();
        }
    }
}
