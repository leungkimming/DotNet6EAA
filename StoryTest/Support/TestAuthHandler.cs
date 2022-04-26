using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace P6.StoryTest {
    public class TestAuthHandler : AuthenticationHandler<TestAuthHandlerOptions> {
        public const string UserId = "UserId";

        public const string AuthenticationScheme = "SpecFlow";

        public TestAuthHandler(
            IOptionsMonitor<TestAuthHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock) {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
            List<Claim> claims;

            if (Context.Request.Headers.TryGetValue(UserId, out var userId)) {
                claims = new List<Claim> { new Claim(ClaimTypes.Name, userId) };
            } else {
                claims = new List<Claim> { new Claim(ClaimTypes.Name, TestHelper.LogonId) };
            }

            // Extract User ID from the request headers if it exists,
            // otherwise use the default User ID from the options.
            if (Context.Request.Headers.TryGetValue("Authorization", out var auth)) {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, auth[0]));
            } else {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, AuthenticationScheme));
            }

            claims.AddRange(TestHelper.claims);

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
