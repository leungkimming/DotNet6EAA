using Microsoft.AspNetCore.Mvc;
using API.Jwt;
using Common.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Service.Users;
using Microsoft.AspNetCore.Antiforgery;
using Common.DTOs;

namespace API.Controllers;

[Route("Login")]
[ApiController]
[IgnoreAntiforgeryToken]
public class LoginController : ControllerBase
{
    private IConfiguration _conf { get; set; }
    private readonly IJWTUtil jwtUtil;
    private readonly UserService _service; 
    private IAntiforgery antiforgery;
    public LoginController(IConfiguration config,
        IJWTUtil _jwtUtil, UserService service,
        IAntiforgery antiForgery)
    {
        _conf = config;
        jwtUtil = _jwtUtil;
        _service = service;
        antiforgery = antiForgery;
    }

    [HttpGet(Name = "Login")]
    public async Task<IActionResult> Get()
    {
        JwtSecurityToken jwtToken;
        string token;

        if (HttpContext.User.Identity.Name == "" || HttpContext.User.Identity.Name == null) {
            throw new InvalidUserException();
        }

        if (jwtUtil.ValidateToken(HttpContext.Request, out jwtToken, out token))
        {
            return Ok(new AuthResult()
            {
                Token = token,
                Success = true,
                RefreshToken = ""
            });
        }

        List<Claim>? claims = _service.GetUserClaims(HttpContext.User.Identity.Name);

        ClaimsIdentity claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
        Array.ForEach(claims.Where(c => c.Type == ClaimTypes.Role).ToArray(),
            c => claimsIdentity.AddClaim(c));

        AntiforgeryTokenSet? tokens = antiforgery.GetAndStoreTokens(HttpContext);
        HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });

        return Ok(jwtUtil.GenerateJwtToken(HttpContext.User.Identity.Name, claims));
    }
}
