using System.Security.Claims;

namespace API {
    /// <summary>
    /// The interface of all custom requirements.
    /// This interface defines ProcessAuthorize to authorize user with custom AccessCodes or others.
    /// Any RequirementHandlers will call this interface and method to process the authorization.
    /// Implement ProcessAuthorize and return the result to handler.
    /// </summary>
    public interface ICustomRequirement {
        bool ProcessAuthorize(ClaimsPrincipal? user, RequirementDefinitionBase? definitions);
    }
}
