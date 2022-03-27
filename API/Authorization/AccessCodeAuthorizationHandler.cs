using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using API.Jwt;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace API.Authorization
{
    internal class AccessCodeAuthorizationHandler : AuthorizationHandler<AccessCodeRequirement>
    {
        private readonly ILogger<AccessCodeAuthorizationHandler> _logger;
        private readonly IJWTUtil jwtUtil;
        private string accessCodes;

        public AccessCodeAuthorizationHandler(ILogger<AccessCodeAuthorizationHandler> logger,
        IJWTUtil _jwtUtil)
        {
            _logger = logger;
            jwtUtil = _jwtUtil;
            accessCodes = "";
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            AccessCodeRequirement requirement)
        {
            string token;
            JwtSecurityToken jwtToken;

            HttpContext httpconcontext = (HttpContext)context.Resource;
            if (jwtUtil.ValidateToken(httpconcontext.Request, out jwtToken, out token) )
            {
                // Avoid hacker using other user's claims. Check the login user against the name in the JWT token
                if (context.User.Identity.Name ==
                    jwtToken.Claims.Where(c => c.Type == "name").Select(c => c.Value).SingleOrDefault())
                {
                    context.User.AddIdentity(new ClaimsIdentity(jwtToken.Claims, "UserRoles"));
                }
            }

            ClaimsPrincipal? userIdentity = context.User;
            if (userIdentity == null) return Task.CompletedTask;

            IEnumerable<Claim>? userClaims = userIdentity.Claims.Where(c => (c.Type == "role" || c.Type == ClaimTypes.Role));
            if (userClaims == null) return Task.CompletedTask;
            if (userClaims.Count() == 0) return Task.CompletedTask;

            foreach (Claim claim in userClaims)
            {
                accessCodes += claim.Value + "/";
            }

            if (accessCodes.Contains(requirement.Role))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
