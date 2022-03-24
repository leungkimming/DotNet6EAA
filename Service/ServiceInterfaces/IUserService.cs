using Common.DTOs;
using System.Security.Principal;

namespace Service {
    /// <summary>
    /// This interface is implementation required for any user services that process the user authorization
    /// The Authorization handler use this interface to handle the authorization
    /// Defines 4 methods contains sync and async methods that provide LoginID and the Identity as parameter
    /// </summary>
    public interface IUserService {
        Task<UserProfileDTO?> GetUserProfileAsync(string loginID);
        Task<UserProfileDTO?> GetUserProfileAsync(IIdentity userIdentity);
        UserProfileDTO? GetUserProfile(string loginID);
        UserProfileDTO? GetUserProfile(IIdentity userIdentity);
    }
}
