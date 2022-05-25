using Microsoft.AspNetCore.Mvc;
using Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service;
using Microsoft.AspNetCore.Antiforgery;

namespace API;

[Route("Login")]
[ApiController]
[IgnoreAntiforgeryToken]
public class LoginController : ControllerBase {
    private IConfiguration _conf { get; set; }
    private readonly IJWTUtil jwtUtil;
    private readonly UserService _service;
    private readonly IAntiforgery antiforgery;
    public LoginController(IConfiguration config,
        IJWTUtil _jwtUtil, UserService service,
        IAntiforgery antiForgery) {
        _conf = config;
        jwtUtil = _jwtUtil;
        _service = service;
        antiforgery = antiForgery;
    }

    [HttpGet(Name = "Login")]
    public async Task<IActionResult> Get(bool force) {
        JwtSecurityToken jwtToken;
        string token;
        AuthResult authResult = new AuthResult();
        RefreshTokenResponse refreshTokenDTO = new RefreshTokenResponse() {
            sRefreshToken = "",
            TokenExpiry = null,
            Success = true,
            Message = ""
        };

        if (HttpContext.User.Identity!.Name == "" || HttpContext.User.Identity.Name == null) {
            throw new InvalidUserException();
        }

        if (!force && jwtUtil.ValidateToken(HttpContext.Request, out jwtToken, out token)) {
            refreshTokenDTO.TokenExpiry = jwtToken.ValidTo;
            refreshTokenDTO.Message = "Not Yet Expired";
            if (HttpContext.User.Identity.Name == jwtToken.Claims
                .Where(c => c.Type == ClaimTypes.Name)
                .Select(c => c.Value).SingleOrDefault()) {
                Array.ForEach(jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role)
                    .ToArray(), c => ((ClaimsIdentity)HttpContext.User.Identity).AddClaim(c));
            }
        } else {
            List<Claim>? claims = _service.GetUserClaims(HttpContext.User.Identity.Name);

            ClaimsIdentity claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            Array.ForEach(claims.Where(c => c.Type == ClaimTypes.Role).ToArray(),
                c => claimsIdentity.AddClaim(c));
            authResult = jwtUtil.GenerateJwtToken(HttpContext.User.Identity.Name, claims);
            HttpContext.Response.Cookies.Append("X-UserRoles", authResult.Token!, 
                new CookieOptions() { HttpOnly = true });
            refreshTokenDTO.sRefreshToken = authResult.RefreshToken;
            refreshTokenDTO.Message = "New Token generated";
        }

        AntiforgeryTokenSet? tokens = antiforgery.GetAndStoreTokens(HttpContext);
        HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken!, new CookieOptions() { HttpOnly = false });

        return Ok(refreshTokenDTO);
    }
}
