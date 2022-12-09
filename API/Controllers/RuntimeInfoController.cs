using Common;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

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

        ///// for testing impersonation ///////
        //string url = "https://itdcom397/IWATest/Api/Values/";
        //var user = ((WindowsIdentity)HttpContext.User.Identity);
        //string result = "";
        //WindowsIdentity.RunImpersonated(user.AccessToken,
        //    () => {
        //        using (var client = new HttpClient(new HttpClientHandler()
        //              { UseDefaultCredentials = true })) {
        //            var response = client.GetAsync(url).Result;
        //            if (response.IsSuccessStatusCode) {
        //                var responseContent = response.Content;
        //                result = responseContent.ReadAsStringAsync().Result;
        //            }
        //        }
        //    }
        //);

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
