using Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace API.Controllers;

[ApiController]
[Route("RuntimeInfo")]
public class RuntimeInfoController : ControllerBase
{
    private IConfiguration _conf { get; set; }
    public RuntimeInfoController(IConfiguration config)
    {
        this._conf = config;
    }

    [HttpGet]
    public RuntimeInfo Get()
    {
        return new RuntimeInfo
        {
            OSArchitecture = RuntimeInformation.OSDescription,
            ProcessArchitecture = RuntimeInformation.RuntimeIdentifier,
            FrameworkDescription = RuntimeInformation.FrameworkDescription,
            SystemVersion = WindowsIdentity.GetCurrent().Name,
            RuntimeDirectory = RuntimeEnvironment.GetRuntimeDirectory(),
            User = HttpContext.User.Identity.Name,
            SQLConnection = _conf.GetConnectionString("DDDConnectionString"),
            Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
        };
    }
}
