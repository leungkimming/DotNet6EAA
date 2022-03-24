using Common.DTOs;
using Common.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Nodes;
using Utilities;

namespace API {
    public sealed class CustomAuthorizeRequirement : IAuthorizationRequirement {

        /// <summary>
        /// To check if the request user is an authorized user using IUserService.
        /// </summary>
        /// <param name="context">The context to Authorize</param>
        /// <returns>A flag if this user is authorized</returns>
        public async Task<bool> AuthorizeRequest(
            AuthorizationHandlerContext context, 
            IHttpContextAccessor contextAccessor, 
            IUserService userService) {

            IIdentity? currentUser = context.User.Identity;

            if (currentUser is not null && currentUser.IsAuthenticated) {

                var customIdentity = await TryGetAuthenticatedUserIdentity(contextAccessor, currentUser, userService);

                if (!customIdentity.IsAuthorized) {
                    throw new CustomException(ErrorRegistry.E1001, "Not authorized user");
                }

                if (customIdentity.UserIdentity is null) {
                    return false;
                }

                context.User.AddIdentity(customIdentity.UserIdentity);

                return true;
            }

            return ServerSettings.Environment == EnvironmentType.Production ? false : true;
        }

        /// <summary>
        /// Reconstruct the methods as async so give better performance
        /// </summary>
        /// <param name="contextAccessor">The HttpContext accessor to get services in HttpContext</param>
        /// <param name="currentUser">The current user identity</param>
        /// <param name="userService">The IUserService to handle the process of authorization</param>
        /// <returns>A tuple that contains IsAuthorized to indicate the authorization and UserIdentity 
        /// to indicate the custom claims identity</returns>
        private async Task<(bool IsAuthorized, UserIdentity? UserIdentity)> TryGetAuthenticatedUserIdentity(
            IHttpContextAccessor contextAccessor,
            IIdentity currentUser,
            IUserService userService) {

            var options = contextAccessor?.HttpContext?.RequestServices.GetService<IOptions<JsonOptions>>();

            var loginID = currentUser.Name is not null ? currentUser.Name : string.Empty;
            UserProfileDTO? userProfile = null;

            // If the user identity not exists in the GridCommon, he will not be the trench work contractor web application user.
            userProfile = await userService.GetUserProfileAsync(loginID);

            if (userProfile is null ||
                userProfile.AccessCodes.Count == 0 ||
                userProfile.UserRoles.Count == 0) {
                return (false, null);
            }

            // Serialize the AccessCodes and UserRoles into Json string and set into claims
            JsonNode? userDataClaimValue = JsonSerializer.SerializeToNode(
                new UserClaims(
                    userProfile.AccessCodes.ToHashSet(),
                    userProfile.UserRoles.Select(r => r.Description).ToHashSet()),
                options?.Value.JsonSerializerOptions);
            string userDataClaimString = userDataClaimValue is not null ? userDataClaimValue.ToString() : string.Empty;

            var claims = new List<Claim>() {
                    new Claim(ClaimTypes.UserData, userDataClaimString),
                    new Claim(ClaimTypes.NameIdentifier, userProfile.LoginIDDTO.Code),
                    new Claim(ClaimTypes.Name, userProfile.Name)
                };

            return (true, new UserIdentity(claims));
        }
    }
}
