using Common;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace API;

[ApiController]
[Route("RuntimeInfo")]
[IgnoreAntiforgeryToken]
public class RuntimeInfoController : ControllerBase {
    private IConfiguration _conf { get; set; }
    private readonly IJWTUtil jwtUtil;

    public RuntimeInfoController(IConfiguration config,
        IJWTUtil _jwtUtil) {
        this._conf = config;
        jwtUtil = _jwtUtil;
    }

    [HttpGet]
    public RuntimeInfo Get() {
        string token;
        JwtSecurityToken jwtToken;
        string accessCodes = "";
        if (jwtUtil.ValidateToken(HttpContext.Request, out jwtToken, out token)) {
            foreach (Claim claim in jwtToken.Claims.Where(c => (c.Type == "role" || c.Type == ClaimTypes.Role))) {
                accessCodes += claim.Value + "/";
            }
        }
        return new RuntimeInfo {
            OSArchitecture = RuntimeInformation.OSDescription,
            ProcessArchitecture = RuntimeInformation.RuntimeIdentifier,
            FrameworkDescription = RuntimeInformation.FrameworkDescription,
            SystemVersion = WindowsIdentity.GetCurrent().Name,
            RuntimeDirectory = RuntimeEnvironment.GetRuntimeDirectory(),
            User = HttpContext.User.Identity.Name,
            SQLConnection = _conf.GetConnectionString("DDDConnectionString"),
            Configuration = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            AccessRights = accessCodes,
            Environment = _conf.GetSection("AppEnvironment").Value,
            Build = _conf.GetSection("Build").Value
        };
    }
}
