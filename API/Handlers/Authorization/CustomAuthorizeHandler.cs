using Microsoft.AspNetCore.Authorization;
using Service;

namespace API {
    public class CustomAuthorizeHandler : AuthorizationHandler<CustomAuthorizeRequirement>, IHandler {

        private readonly IGridCommonService _gridCommonService;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="gridCommonService">Injected GridCommon2 service</param>
        /// <param name="contextAccessor">Injected context to get services from HttpContext</param>
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
