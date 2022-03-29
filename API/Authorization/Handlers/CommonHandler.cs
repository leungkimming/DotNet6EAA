using Microsoft.AspNetCore.Authorization;
using Service;

namespace API {
    /// <summary>
    /// The common handler for all ICustomRequirements to process the procedure of Policy-based Authorization.
    /// Inject all Definitions customized by developer that implements 
    /// from RequirementDefinitionBase to get all needed access fields.
    /// </summary>
    public class CommonHandler : IAuthorizationHandler, IHandler {
        private readonly IEnumerable<RequirementDefinitionBase> _definitions; 
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CommonHandler(
            IEnumerable<RequirementDefinitionBase> definitions, 
            IEnumerable<IUserService> userServices, 
            IHttpContextAccessor contextAccessor) {

            _definitions = definitions;
            _userService = userServices.Single(t => t.GetType() == typeof(TestUserService));
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context) {
            var pendingRequirements = context.PendingRequirements.ToList();
            pendingRequirements.ForEach(async requirement => {
                if (requirement.GetType() == typeof(DefaultRequirement)) {
                    if (!await ((DefaultRequirement)requirement).AuthorizeRequest(context, _contextAccessor, _userService)) {
                        context?.Fail();
                    }
                } else {
                    var definition = _definitions.SingleOrDefault(d => 
                        d.GetType().BaseType?.GetGenericArguments().First() == requirement.GetType());
                    if (typeof(ICustomRequirement).IsAssignableFrom(requirement.GetType())) {
                        if (((ICustomRequirement)requirement).ProcessAuthorize(context?.User, definition)) {
                            context?.Succeed(requirement);
                        } else {
                            context?.Fail();
                        }
                    }
                }
            });
        }
    }
}
