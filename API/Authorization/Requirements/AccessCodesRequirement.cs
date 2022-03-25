using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API {
    public sealed class AccessCodesRequirement : IAuthorizationRequirement, ICustomRequirement {
        public bool ProcessAuthorize(ClaimsPrincipal? user, RequirementDefinitionBase definition) {
            var accessCodes = (user?.Identities
                .SingleOrDefault(i => i.GetType() == typeof(UserIdentity), new UserIdentity()) as UserIdentity)
                ?.CustomClaims?.AccessCodes;
            return definition.DefinitionList.All(i => accessCodes?.Contains(i) ?? false);
        }
    }
}
