using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace API {
    internal class AccessCodePolicyProvider : IAuthorizationPolicyProvider {
        const string POLICY_PREFIX = "Role";
        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public AccessCodePolicyProvider(IOptions<AuthorizationOptions> options) {
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName) {
            if (policyName.StartsWith(POLICY_PREFIX, StringComparison.OrdinalIgnoreCase)) {
                var policy = new AuthorizationPolicyBuilder();
                policy.AddRequirements(new AccessCodeRequirement(policyName.Substring(POLICY_PREFIX.Length)));
                policy.RequireAuthenticatedUser();
                return Task.FromResult(policy.Build());
            }

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
