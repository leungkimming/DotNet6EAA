using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API {
    public class TestRequirement : IAuthorizationRequirement, ICustomRequirement {
        public bool ProcessAuthorize(ClaimsPrincipal? user, RequirementDefinitionBase definitions) {
            var accessCodes = new HashSet<string>();
            accessCodes.Add("Test1");
            accessCodes.Add("Test2");
            accessCodes.Add("Test3");
            return definitions.DefinitionList.All(i => accessCodes.Contains(i));
        }
    }
}
