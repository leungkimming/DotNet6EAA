using HKElectric.GridCommon2.CoreService;
using Common.Utilities;
using Common.DTOs;

namespace Service {
    public class GridCommonService : IGridCommonService {

        private readonly CoreService _coreClient;

        public GridCommonService() {
            _coreClient = new CoreServiceClient();
        }

        public async Task<UserProfileDTO?> GetUserProfile(string loginID) {
            var getUserProfileRequest = new GetUserProfileWithSystemCodeRequest();
            getUserProfileRequest.Code = loginID;
            getUserProfileRequest.SystemCode = ServerSettings.SystemCode;
            var userPO = await _coreClient.RequestAsync(getUserProfileRequest) as GetUserProfileWithSystemCodeResponse;
            var userProfileDTO = TranslateToDTO(userPO?.UserProfile);
            return userProfileDTO;
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
