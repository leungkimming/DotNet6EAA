using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API {
    public class TestController : AuthControllerBase {
        [HttpGet]
        [Authorize(Policy = nameof(TestRequirement))]
        public IActionResult Test() {
            return Ok();
        }
    }
}
