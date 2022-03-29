using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API {
    /// <summary>
    /// This is a Test requirement for how to define the custom requirement for all the use case
    /// Must implement ICustomRequirement so that the Authorization can work
    /// Set any needed criterias into RequirementDefinitionBase.DefinitionList as the requirement criterias
    /// </summary>
    public sealed class PolicyTestRequirement : IAuthorizationRequirement, ICustomRequirement {
        public bool ProcessAuthorize(ClaimsPrincipal? user, RequirementDefinitionBase? definitions) {
            // These codes is to simulate get the access codes from user.
            var accessCodes = new HashSet<string>();
            accessCodes.Add("Test1");
            accessCodes.Add("Test2");
            accessCodes.Add("Test3");

            // In fact, just needs this result to process the authorization, before this, get the access codes from user.
            return definitions?.DefinitionList.All(i => accessCodes.Contains(i)) ?? false;
        }
    }
}
