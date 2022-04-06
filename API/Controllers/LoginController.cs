using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Jwt;
using Common.Shared;
using Microsoft.AspNetCore.Authentication.Negotiate;
using System.Security.Claims;
using Service.Users;
using Microsoft.AspNetCore.Antiforgery;

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
        if (HttpContext.User.Identity.Name == "" || HttpContext.User.Identity.Name == null) {
            throw new InvalidUserException();
        }

        List<Claim>? claims = _service.GetUserClaims(HttpContext.User.Identity.Name);

        ClaimsIdentity claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
        Array.ForEach(claims.Where(c => c.Type == ClaimTypes.Role).ToArray(),
            c => claimsIdentity.AddClaim(c));

        AntiforgeryTokenSet? tokens = antiforgery.GetAndStoreTokens(HttpContext);
        System.Diagnostics.Debug.WriteLine($"Path={HttpContext.Request.Path.Value} User={HttpContext.User.Identity.Name}, NewToken={tokens.RequestToken}");
        HttpContext.Response.Cookies.Append("XSRF-TOKEN", tokens.RequestToken, new CookieOptions() { HttpOnly = false });

        return Ok(jwtUtil.ValidateRefreshJTWToken(HttpContext.Request, HttpContext.User.Identity.Name, claims));
    }
}
