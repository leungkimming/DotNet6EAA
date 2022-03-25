using Common.DTOs;
using System.Security.Principal;

namespace Service {
    /// <summary>
    /// This interface is implementation required for any user services that process the user authorization
    /// The Authorization handler use this interface to handle the authorization
    /// Defines 4 methods contains sync and async methods 
    /// that provide LoginID/Token-String and the IIdentity from HttpContext or custom ways as parameter
    /// </summary>
    public interface IUserService {
        /// <summary>
        /// Asynchronous method to GetUserProfile from the given login ID or login token
        /// </summary>
        /// <param name="loginData">The login ID or login token to get user profile</param>
        /// <returns>The UserProfileDTO that contains AccessCodes and UserRoles</returns>
        Task<UserProfileDTO?> GetUserProfileAsync(string loginData);

        /// <summary>
        /// Asynchronous method to GetUserProfile from the given IIdentity
        /// </summary>
        /// <param name="userIdentity">The IIdentity from HttpContext</param>
        /// <returns>The UserProfileDTO that contains AccessCodes and UserRoles</returns>
        Task<UserProfileDTO?> GetUserProfileAsync(IIdentity userIdentity);

        /// <summary>
        /// Synchronous method to GetUserProfile from the given login ID or login token
        /// </summary>
        /// <param name="loginData">The login ID or login token to get user profile</param>
        /// <returns>The UserProfileDTO that contains AccessCodes and UserRoles</returns>
        UserProfileDTO? GetUserProfile(string loginData);

        /// <summary>
        /// Synchronous method to GetUserProfile from the given IIdentity
        /// </summary>
        /// <param name="userIdentity">The IIdentity from HttpContext</param>
        /// <returns>The UserProfileDTO that contains AccessCodes and UserRoles</returns>
        UserProfileDTO? GetUserProfile(IIdentity userIdentity);
    }
}
