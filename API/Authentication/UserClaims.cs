namespace API {
    public class UserClaims {
        /// <summary>
        /// The Access Codes that current authenticated user granted.
        /// </summary>
        public HashSet<string> AccessCodes { get; private set; }

        /// <summary>
        /// The roles that current authenticated user granted.
        /// </summary>
        public HashSet<string> Roles { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TrenchWorkContractorPrincipal"/> class from the specified identity.
        /// </summary>
        /// <param name="accessCodes"></param>
        /// <param name="roles"></param>
        public UserClaims(HashSet<string> accessCodes, HashSet<string> roles) {
            AccessCodes = accessCodes;
            Roles = roles;
        }

        /// <summary>
        /// Indicate if the user is permitted to access the specified function.
        /// </summary>
        /// <param name="role">The access code that specified function required.</param>
        /// <returns></returns>
        public bool IsInRole(string role) {
            return AccessCodes.Contains(role);
        }
    }
}
