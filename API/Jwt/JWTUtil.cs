using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Common.DTOs;
using Business.JWT;

namespace API.Jwt
{
    public class JWTUtil : IJWTUtil
    {
        public string Secret { get; set; }
        private byte[] key { get; set; }
        private TokenValidationParameters validationParm { get; set; }
        private JwtSecurityTokenHandler tokenHandler { get; set; }
        public JWTUtil(IConfiguration config)
        {
            key = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:Secret").Value);
            validationParm = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
            tokenHandler = new JwtSecurityTokenHandler();
        }

        public AuthResult ValidateRefreshJTWToken(HttpRequest request, string userId, List<Claim> claims)
        {
            AuthResult authResult = null;
            string token;
            JwtSecurityToken jwtToken;

            if (ValidateToken(request, out jwtToken, out token))
            {
                authResult = new AuthResult()
                {
                    Token = token,
                    Success = true,
                    RefreshToken = ""
                };
            } else
            {
                authResult = GenerateJwtToken(key, userId, claims);
            }

            return authResult;
        }
        public bool ValidateToken(HttpRequest request, out JwtSecurityToken jwtToken, out string token) 
        {
            var tokens = request.Headers["X-UserRoles"];
            if (tokens.Count() > 0)
            {
                token = tokens[0];
                try
                {
                    tokenHandler.ValidateToken(token, validationParm,
                        out SecurityToken validatedToken);
                    jwtToken = (JwtSecurityToken)validatedToken;

                    if (jwtToken.ValidTo.Subtract(DateTime.UtcNow) < new TimeSpan(0, 0, 10))
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    jwtToken = null;
                    return false;
                }
            } else
            {
                token = null;
                jwtToken = null;
                return false;
            }
            return true;
        }
        private AuthResult GenerateJwtToken(byte[] key, string user, List<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10), // for testing logics after expiry
              //Expires = DateTime.UtcNow.AddDays(1),  // valid for 1 day
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsUsed = false,
                IsRevorked = false,
                UserId = user,
                AddedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(1),
                Token = RandomString(35) + Guid.NewGuid()
            };
            // to implement refresh Token, you need to insert refreshToken to DB and create refresh Token service.
            return new AuthResult()
            {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }
        private string RandomString(int length)
        {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
