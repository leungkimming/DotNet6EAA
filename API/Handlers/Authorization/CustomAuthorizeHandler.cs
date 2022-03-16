using Microsoft.AspNetCore.Authorization;
using Service;

namespace API {
    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeRequirement>, IHandler {

        private readonly IGridCommonService _gridCommonService;

        public CustomAuthorizeHandler(IGridCommonService gridCommonService) {
            _gridCommonService = gridCommonService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CustomAuthorizeRequirement requirement) {

            if (requirement.AuthorizeRequest(context, _gridCommonService)) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
