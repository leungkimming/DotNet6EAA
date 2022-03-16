using System.Security.Claims;
using System.Text.Json;

namespace API {
    public class UserIdentity : ClaimsIdentity {
        /// <summary>
        /// Indicate the user name of the current authenticated contractor user.
        /// </summary>
        public string? LoginID { get; private set; }

        /// <summary>
        /// Indicate the vendor number of the current authenticated contractor user.
        /// </summary>
        public string? VendorNo { get; private set; }

        /// <summary>
        /// Indicate the custom claims to receive the UserRoles and AccessCodess of the current authenticated contractor user.
        /// </summary>
        public UserClaims? CustomClaims { get; private set; }

        /// <summary>
        /// The constructor of TrenchWorkContractorIdentity, 
        /// and to construct the base ClaimsIdentity using custom
        /// IEnumerbale<Claim> to define the custom claims
        /// </summary>
        /// <param name="claims">The IEnumerbale<Claim> of customization</param>
        public UserIdentity(IEnumerable<Claim> claims) : base(claims) {
            LoginID = claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            VendorNo = claims.SingleOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
            CustomClaims = GetCustomClaim();
        }

        /// <summary>
        /// Get the UserRoles and AccessCodes from the custom UserData claim
        /// use Json string to store the Hashsets, deserialize into TrenchWorkContractorClaims
        /// in this method to get TrenchWorkContractorClaims
        /// </summary>
        /// <returns>The TrenchWorkContractorClaims type that contains Hashsets of UserRoles and AccessCodes</returns>
        public UserClaims? GetCustomClaim() {
            var claimValue = Claims.SingleOrDefault(c => c.Type == ClaimTypes.UserData)?.Value;
            return claimValue is not null ?
                JsonSerializer.Deserialize<UserClaims>(claimValue) : 
                new UserClaims(new HashSet<string>(), new HashSet<string>());
        }
    }
}