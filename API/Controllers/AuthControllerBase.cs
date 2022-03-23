using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API {
    /// <summary>
    /// Base Controller for Window Authentication 
    /// </summary>
    /// 
    [ApiController]
    [Route("[controller]/[action]")]
    [Produces(MediaTypeNames.Application.Json)]
    public class AuthControllerBase : ControllerBase {

        /// <summary>
        /// To indicate the authenticated contractor user principal
        /// </summary>
        protected UserClaims? AuthenticatedUserClaims {
            get {
                var customClaims = (ControllerContext.HttpContext.User.Identities
                    .SingleOrDefault(i => i.GetType() == typeof(UserIdentity), new UserIdentity())
                    as UserIdentity)
                    ?.CustomClaims;
                return customClaims;
            }
        }

        /// <summary>
        /// To indicate the authenticated contractor user identity
        /// </summary>
        protected UserIdentity? AuthenticatedUserIdentity {
            get {
                var customIdentity = ControllerContext.HttpContext.User.Identities
                    .SingleOrDefault(i => i.GetType() == typeof(UserIdentity), new UserIdentity())
                    as UserIdentity;
                return customIdentity;
            }
        }

        public AuthControllerBase() {
        }

        protected string GetCurrentID() {
            WindowsIdentity? windowsIdentity = ControllerContext.HttpContext.User.Identity as WindowsIdentity;
            string userNameWithDomin = windowsIdentity?.Name is not null ? windowsIdentity.Name : string.Empty;

            return userNameWithDomin;
        }
    }
}
