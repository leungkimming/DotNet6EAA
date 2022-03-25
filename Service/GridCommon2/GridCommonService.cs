using HKElectric.GridCommon2.CoreService;
using Common.DTOs;
using System.Security.Principal;
using Utilities;

namespace Service {
    /// <summary>
    /// GridCommon2 is an example for how to use IUserService
    /// </summary>
    public class GridCommonService : IUserService {

        private readonly CoreService _coreClient;

        public GridCommonService() {
            _coreClient = new CoreServiceClient();
        }

        public UserProfileDTO? GetUserProfile(string loginData) {
            var getUserProfileRequest = new GetUserProfileWithSystemCodeRequest();
            getUserProfileRequest.Code = loginData;
            getUserProfileRequest.SystemCode = ServerSettings.SystemCode;
            var userPO = _coreClient.Request(getUserProfileRequest) as GetUserProfileWithSystemCodeResponse;
            var userProfileDTO = TranslateToDTO(userPO?.UserProfile);
            return userProfileDTO;
        }

        public UserProfileDTO? GetUserProfile(IIdentity userIdentity) {
            if (userIdentity is null) {
                return null;
            } else {
                var getUserProfileRequest = new GetUserProfileWithSystemCodeRequest();
                string? loginID = (userIdentity as WindowsIdentity)?.Name;
                getUserProfileRequest.Code = loginID;
                getUserProfileRequest.SystemCode = ServerSettings.SystemCode;
                var userPO = _coreClient.Request(getUserProfileRequest) as GetUserProfileWithSystemCodeResponse;
                var userProfileDTO = TranslateToDTO(userPO?.UserProfile);
                return userProfileDTO;
            }
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(string loginData) {
            var getUserProfileRequest = new GetUserProfileWithSystemCodeRequest();
            getUserProfileRequest.Code = loginData;
            getUserProfileRequest.SystemCode = ServerSettings.SystemCode;
            var userPO = await _coreClient.RequestAsync(getUserProfileRequest) as GetUserProfileWithSystemCodeResponse;
            var userProfileDTO = TranslateToDTO(userPO?.UserProfile);
            return userProfileDTO;
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(IIdentity userIdentity) {
            if (userIdentity is null) {
                return null;
            } else {
                var getUserProfileRequest = new GetUserProfileWithSystemCodeRequest();
                string? loginID = (userIdentity as WindowsIdentity)?.Name;
                getUserProfileRequest.Code = loginID;
                getUserProfileRequest.SystemCode = ServerSettings.SystemCode;
                var userPO = await _coreClient.RequestAsync(getUserProfileRequest) as GetUserProfileWithSystemCodeResponse;
                var userProfileDTO = TranslateToDTO(userPO?.UserProfile);
                return userProfileDTO;
            }
        }

        private UserProfileDTO? TranslateToDTO(UserProfilePO? userProfilePO) {

            if (userProfilePO is null) {
                return null;
            }

            var userProfileDTO = new UserProfileDTO() {
                LoginIDDTO = new EntityCodeDTO() {
                    Code = userProfilePO.LoginIDPO.Code,
                    RecordVersion = userProfilePO.LoginIDPO.RecordVersion
                },
                Name = userProfilePO.Name,
                EffectiveEndDate = userProfilePO.EffectiveEndDate,
                LastUpdateBy = userProfilePO.LastUpdatedBy,
                LastUpdatedDateTime = userProfilePO.LastUpdatedDateTime,
                IsSysAdmin = userProfilePO.IsSysAdmin
            };

            foreach (var userRole in userProfilePO.UserRoleTypeList.Items) {
                userProfileDTO.UserRoles.Add(new UserRoleDTO() {
                    Code = userRole.Code,
                    Description = userRole.Description,
                    AccessCodes = userRole.AccessCodes,
                    EffectiveEndDate = userRole.EffectiveEndDate,
                    EffectiveStartDate = userRole.EffectiveStartDate
                });
            }

            return userProfileDTO;
        }

    }
}
