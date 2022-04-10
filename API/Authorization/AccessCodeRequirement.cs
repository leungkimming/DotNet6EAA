using Microsoft.AspNetCore.Authorization;

namespace API {
    internal class AccessCodeRequirement : IAuthorizationRequirement {
        public string Role { get; private set; }

        public AccessCodeRequirement(string role) { Role = role; }
    }
}