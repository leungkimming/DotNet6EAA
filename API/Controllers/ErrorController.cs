using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("Error")]
    [IgnoreAntiforgeryToken]
    public class ErrorController : ControllerBase
    {
        public readonly ILogger<ErrorHandler> _logger;
        public string? RequestId { get; set; }
        public ErrorController(ILogger<ErrorHandler> logger)
        {
            this._logger = logger;
        }

        [AllowAnonymous]
        [HttpPost(Name = "Error")]
        public IActionResult Error()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            var context = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (context != null)
            {
                var ex = context.Error;
                _logger.LogError(9999, context.Path + ex.Message + "\n" + ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError, String.Format(
                    @"Service temporarily interrupted.Please report the problem to IT Help Desk with Trace Id ""{0}""",
                    RequestId));
            }

            return Ok(Problem(statusCode: 200, detail: ""));
        }
    }
}