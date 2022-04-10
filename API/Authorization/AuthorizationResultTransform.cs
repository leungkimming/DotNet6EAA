using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Net;
using System.Text.Json;

namespace API {
    public class AuthorizationResultTransformer : IAuthorizationMiddlewareResultHandler {
        private readonly IAuthorizationMiddlewareResultHandler _handler;
        public readonly ILogger<AuthorizationResultTransformer> _logger;

        public AuthorizationResultTransformer(ILogger<AuthorizationResultTransformer> logger) {
            _handler = new AuthorizationMiddlewareResultHandler();
            _logger = logger;
        }

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult) {
            if (policyAuthorizationResult.Forbidden && policyAuthorizationResult.AuthorizationFailure != null) {
                if (policyAuthorizationResult.AuthorizationFailure.FailedRequirements.Any(requirement => requirement is AccessCodeRequirement)) {
                    AccessCodeRequirement req = (AccessCodeRequirement)authorizationPolicy.Requirements.FirstOrDefault(requirement => requirement is AccessCodeRequirement);

                    string _message = String.Format(@"User Id ""{0}"" ""{1}"" to resource ""{2}"" denied, missing access code ""{3}""",
                    httpContext.User.Identity.Name,
                    httpContext.Request.Method,
                    httpContext.Request.Host + httpContext.Request.PathBase + httpContext.Request.Path,
                    req.Role);
                    _logger.LogError(9998, _message);
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(new { message = "Access denied. Please call IT Service Desk." }));
                    return;
                }
                // Event Log: User Id "HEH\41776" "GET" to resource "localhost:44355/users" denied, missing access code "AA01"
                // Other transformations here
            }
            await _handler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
        }
    }
}
