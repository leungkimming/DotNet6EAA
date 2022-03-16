using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Principal;

namespace API {
    public class ImpersonationFilterAttribute : ActionFilterAttribute {

        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
#pragma warning disable CA1416 // Validate platform compatibility
            var accessToken = (context.HttpContext.User.Identity as WindowsIdentity)?.AccessToken;
            if (accessToken is not null) {
                await WindowsIdentity.RunImpersonatedAsync(accessToken, async () => {
                    await next();
                });
            } else {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await context.HttpContext.Response.StartAsync();
            }
#pragma warning restore CA1416 // Validate platform compatibility
        }
    }
}
