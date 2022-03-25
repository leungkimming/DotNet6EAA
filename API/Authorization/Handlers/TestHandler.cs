using Microsoft.AspNetCore.Authorization;

namespace API {
    public class TestHandler : AuthorizationHandler<TestRequirement>, IHandler {

        private readonly RequirementDefinitionBase _definitions;

        public TestHandler(IEnumerable<RequirementDefinitionBase> requirementDefinitions) {
            _definitions = requirementDefinitions.Single(d => d.GetType() == typeof(TestDefinition));
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, TestRequirement requirement) {
            if (requirement.ProcessAuthorize(context?.User, _definitions)) {
                context?.Succeed(requirement);
            } else {
                context?.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
