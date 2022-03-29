using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API {
    /// <summary>
    /// This is a test controller for calling custom Policy-based Authorization
    /// </summary>
    public class PolicyTestController : AuthControllerBase {
        [HttpGet]
        [Authorize(Policy = nameof(PolicyTestRequirement))]
        public IActionResult Test() {
            return Ok("Has Authorize policy.");
        }

        [HttpGet]
        public IActionResult TestNoAuth() {
            return Ok("No Authorize policy.");
        }
    }
}
