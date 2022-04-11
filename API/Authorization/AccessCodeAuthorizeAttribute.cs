using Microsoft.AspNetCore.Authorization;

namespace API {
    // This attribute derives from the [Authorize] attribute, adding 
    // the ability for a user to specify an 'age' paratmer. Since authorization
    // policies are looked up from the policy provider only by string, this
    // authorization attribute creates is policy name based on a constant prefix
    // and the user-supplied age parameter. A custom authorization policy provider
    // (`MinimumAgePolicyProvider`) can then produce an authorization policy with 
    // the necessary requirements based on this policy name.
    internal class AccessCodeAuthorizeAttribute : AuthorizeAttribute {
        const string POLICY_PREFIX = "Role";

        public AccessCodeAuthorizeAttribute(string role) => Role = role;

        // Get or set the Age property by manipulating the underlying Policy property
        public string Role {
            get {
                string role = Policy.Substring(POLICY_PREFIX.Length);
                if (role != "") {
                    return role;
                }
                return "";
            }
            set {
                Policy = $"{POLICY_PREFIX}{value.ToString()}";
            }
        }
    }
}