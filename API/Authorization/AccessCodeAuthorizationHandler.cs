using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace API {
    internal class AccessCodeAuthorizationHandler : AuthorizationHandler<AccessCodeRequirement> {
        private readonly ILogger<AccessCodeAuthorizationHandler> _logger;
        private readonly IJWTUtil jwtUtil;
        private string accessCodes;

        public AccessCodeAuthorizationHandler(ILogger<AccessCodeAuthorizationHandler> logger,
        IJWTUtil _jwtUtil) {
            _logger = logger;
            jwtUtil = _jwtUtil;
            accessCodes = "";
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AccessCodeRequirement requirement) {
            string token;
            JwtSecurityToken jwtToken;
            List<Claim> effectiveClaims = new List<Claim>();
            HttpContext httpconcontext = (HttpContext)context.Resource;
            if (jwtUtil.ValidateToken(httpconcontext.Request, out jwtToken, out token)) {
                // Avoid hacker using other user's claims. Check the login user against the name in the JWT token
                if (context.User.Identity.Name ==
                    jwtToken.Claims.Where(c => c.Type == ClaimTypes.Name).Select(c => c.Value).SingleOrDefault()) {
                    Array.ForEach(jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role)
                        .ToArray(), c => ((ClaimsIdentity)context.User.Identity).AddClaim(c));
                }
            }

            ClaimsPrincipal? userIdentity = context.User;
            if (userIdentity == null) return Task.CompletedTask; //Unauthorized anonymous user

            IEnumerable<Claim>? userClaims = userIdentity.Claims.Where(c => c.Type == ClaimTypes.Role);

            foreach (Claim claim in userClaims) {
                accessCodes += claim.Value + "/";
            }

            if (accessCodes.Contains(requirement.Role)) {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
