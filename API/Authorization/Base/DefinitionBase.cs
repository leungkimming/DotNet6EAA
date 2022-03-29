using Microsoft.AspNetCore.Authorization;

namespace API {
    /// <summary>
    /// The <see cref="DefinitionBase{TRequirement}"/> class is the base class for all RequirementDefinitions.
    /// Must inherit this class for every ICustomRequirements.
    /// The generic argument will be used in reflection to find the correct requirement.
    /// </summary>
    /// <typeparam name="TRequirement">The requirement that this definition belongs to.</typeparam>
    public abstract class DefinitionBase<TRequirement> : RequirementDefinitionBase where TRequirement : IAuthorizationRequirement {
    }
}
