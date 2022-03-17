using Microsoft.AspNetCore.Authorization;
using Service;

namespace API {
    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeRequirement>, IHandler {

        private readonly IGridCommonService _gridCommonService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CustomAuthorizeHandler(IGridCommonService gridCommonService, IHttpContextAccessor contextAccessor) {
            _gridCommonService = gridCommonService;
            _contextAccessor = contextAccessor;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            CustomAuthorizeRequirement requirement) {

            if (requirement.AuthorizeRequest(context, _contextAccessor, _gridCommonService)) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
