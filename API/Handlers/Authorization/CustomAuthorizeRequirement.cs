using Common.DTOs;
using Common.Shared;
using Common.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace API {
    public class CustomAuthorizeRequirement : IAuthorizationRequirement {

        /// <summary>
        /// To check if the request user is an authorized user using GridCommon2.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual bool AuthorizeRequest(AuthorizationHandlerContext context, IHttpContextAccessor contextAccessor, IGridCommonService gridCommonService) {
            IIdentity? currentUser = context.User.Identity;

            if (currentUser is not null && currentUser.IsAuthenticated) {

                if (!TryGetAuthenticatedUserPrincipal(contextAccessor, currentUser, gridCommonService, out var userIdentity)) {
                    throw new CustomException(ErrorRegistry.E1001, "Not authorized user");
                }

                if (userIdentity is null) {
                    return false;
                }

                context.User.AddIdentity(userIdentity);

                return true;
            }

            return ServerSettings.Environment == EnvironmentType.Production ? false : true;
        }

        private bool TryGetAuthenticatedUserPrincipal(
            IHttpContextAccessor contextAccessor,
            IIdentity currentUser,
            IGridCommonService userService,
            out UserIdentity? userIdentity) {

            userIdentity = null;

            var options = contextAccessor?.HttpContext?.RequestServices.GetService<IOptions<JsonOptions>>();

            var loginID = currentUser.Name is not null ? currentUser.Name : string.Empty;
            UserProfileDTO? userProfile = null;

            // If the user identity not exists in the GridCommon, he will not be the trench work contractor web application user.
            userProfile = userService.GetUserProfile(loginID).GetAwaiter().GetResult();

            if (userProfile is null ||
                userProfile.AccessCodes.Count == 0 ||
                userProfile.UserRoles.Count == 0) {
                return false;
            }

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

            userIdentity = new UserIdentity(claims);

            return true;
        }
    }
}
