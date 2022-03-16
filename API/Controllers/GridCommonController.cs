using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace API {
    public class GridCommonController : AuthControllerBase {

        [HttpGet]
        public object GetUser() {
            var result = new {
                name = AuthenticatedUserIdentity?.Name,
                claims = AuthenticatedUserIdentity?.Claims.ToList()
            };
            return result;
        }
    }
}
