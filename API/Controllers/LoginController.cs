using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Jwt;
using Common.Shared;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Common.DTOs;
using System.Security.Claims;
using Service.Users;

namespace API.Controllers;

[ApiController]
[Route("Login")]
[Authorize(AuthenticationSchemes = NegotiateDefaults.AuthenticationScheme)]
public class LoginController : ControllerBase
{
    private IConfiguration _conf { get; set; }
    private readonly IJWTUtil jwtUtil;
    private readonly UserService _service;
    public LoginController(IConfiguration config,
        IJWTUtil _jwtUtil, UserService service)
    {
        _conf = config;
        jwtUtil = _jwtUtil;
        _service = service;
    }

    [HttpGet]
    public AuthResult Get()
    {
        AuthResult authResult;

        if (HttpContext.User.Identity.Name == "" || HttpContext.User.Identity.Name == null) {
            throw new InvalidUserException();
        }

        var claims = _service.GetUserClaims(HttpContext.User.Identity.Name);

        //JWTUtil jwtUtil = new JWTUtil(_jwtConfig);
        return jwtUtil.ValidateRefreshJTWToken(HttpContext.Request, HttpContext.User.Identity.Name, claims);
    }
}
