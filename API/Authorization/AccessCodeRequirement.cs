using Microsoft.AspNetCore.Authorization;

namespace API.Authorization
{
    internal class AccessCodeRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public AccessCodeRequirement(string role) { Role = role; }
    }
}