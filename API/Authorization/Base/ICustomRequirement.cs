using System.Security.Claims;

namespace API {
    public interface ICustomRequirement {
        bool ProcessAuthorize(ClaimsPrincipal? user, RequirementDefinitionBase definitions);
    }
}
