using Microsoft.AspNetCore.Authorization;

namespace API {
    public class AccessCodesHandler : AuthorizationHandler<AccessCodesRequirement>, IHandler {

        private readonly RequirementDefinitionBase _definitions;

        public AccessCodesHandler(IEnumerable<RequirementDefinitionBase> requirementDefinitions) {
            _definitions = requirementDefinitions.Single(d => d.GetType() == typeof(AccessCodesDefinition));
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            AccessCodesRequirement requirement) {

            if (requirement.ProcessAuthorize(context?.User, _definitions)) {
                context?.Succeed(requirement);
            } else {
                context?.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
