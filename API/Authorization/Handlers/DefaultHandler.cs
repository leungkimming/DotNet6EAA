using Microsoft.AspNetCore.Authorization;
using Service;

namespace API {
    public class DefaultHandler : AuthorizationHandler<DefaultRequirement>, IHandler {

        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _contextAccessor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userServices">Injected IUserServices</param>
        /// <param name="contextAccessor">Injected context to get services from HttpContext</param>
        public DefaultHandler(IEnumerable<IUserService> userServices, IHttpContextAccessor contextAccessor) {
            // Replace the type to any certain services that implements IUserService that process the user authorization.
            _userService = userServices.Single(t => t.GetType() == typeof(GridCommonService));
            _contextAccessor = contextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            DefaultRequirement requirement) {

            if (await requirement.AuthorizeRequest(context, _contextAccessor, _userService)) {
                context.Succeed(requirement);
            } else {
                context.Fail();
            }
        }
    }
}
