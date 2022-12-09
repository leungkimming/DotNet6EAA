using AutoMapper;
using Common;
using GridCommon2;

namespace Service {
    public class GridCommon2Profile : Profile {
        public GridCommon2Profile() {
            // PO from GC2 lib -> UserProfileDto
            CreateMap<UserProfilePO, UserProfileDto>()
                .ForMember(dest => dest.LoginId, opt => opt.MapFrom<GridCommon2ProfileUserProfilePOLoginIdResolver>())
                .ForMember(dest => dest.AccessCodes, opt => opt.MapFrom<GridCommon2ProfileAccessCodesResolver>())
                .ForMember(dest => dest.BusinessUnit, opt => opt.MapFrom(src => src.BusinessUnit.Code))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom<GridCommon2ProfileRolesResolver>())
                .ForMember(dest => dest.Division, opt => opt.MapFrom(src => src.DivisionCode))
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.DepartmentCode))
                .ForMember(dest => dest.Section, opt => opt.MapFrom(src => src.SectionCode));
        }
    }

    /** 
     * Implementation AutoMapper Resolver Interface. More flexible mapper, which allow DI and complex mapping logic.
     **/
    public class GridCommon2ProfileRolesResolver : IValueResolver<UserProfilePO, UserProfileDto, RoleDto?[]?> {
        private readonly IAppSettings _appSettings;
        public GridCommon2ProfileRolesResolver(IAppSettings appSettings) {
            this._appSettings = appSettings;
        }

        public RoleDto?[]? Resolve(UserProfilePO source, UserProfileDto dest, RoleDto?[]? destMember, ResolutionContext context) {
            return source.UserRoleTypeList.Items.Where(role => role.Code.StartsWith(_appSettings.GridCommon2.Prefix))
                    .Select(role => new Role {
                        AccessCodes = role.AccessCodes.Split(new char[] { '|' }),
                        Code = role.Code,
                        DelegateFrom = role.DelegateFrom,
                        Description = role.Description,
                        EffectiveEndDate = role.EffectiveEndDate.ToString(),
                        EffectiveStartDate = role.EffectiveEndDate.ToString(),
                    })
                    .Select(r => {
                        if (r.Code != null && r.Description != null) {
                            return new RoleDto {
                                Code = r.Code,
                                Description = r.Description
                            };
                        }
                        return null;
                    }).ToArray();
        }
    }

    public class GridCommon2ProfileAccessCodesResolver : IValueResolver<UserProfilePO, UserProfileDto, string[]?> {
        private readonly IAppSettings _appSettings;
        public GridCommon2ProfileAccessCodesResolver(IAppSettings _appSettings) {
            this._appSettings = _appSettings;
        }

        public string[]? Resolve(UserProfilePO src, UserProfileDto dest, string[]? destMember, ResolutionContext context) {
            return src.AccessCodes.Items.Where(c => c.StartsWith(_appSettings.GridCommon2.Prefix)).ToArray();
        }
    }

    // ODC2 GC2 API used
    public class GridCommon2ProfileUserProfilePOLoginIdResolver : IValueResolver<UserProfilePO, UserProfileDto, string?> {
        public string? Resolve(UserProfilePO source, UserProfileDto destination, string? destMember, ResolutionContext context) {
            return source.LoginIDPO.Code?.Split("\\").LastOrDefault();
        }
    }
}
