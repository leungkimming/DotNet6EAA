using HKElectric.GridCommon2.CoreService;
using Common.DTOs;

namespace Service {
    public interface IGridCommonService {
        Task<UserProfileDTO?> GetUserProfile(string loginID);
    }
}
