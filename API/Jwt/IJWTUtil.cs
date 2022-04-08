using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Common.DTOs;
using Business.JWT;

namespace API.Jwt
{
    public interface IJWTUtil
    {
        public bool ValidateToken(HttpRequest request, out JwtSecurityToken jwtToken, out string token);
        public AuthResult GenerateJwtToken(string user, List<Claim> claims);
    }
}
