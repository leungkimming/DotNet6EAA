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
        private SigningCredentials signingCredentials { get; set; }
        private EncryptingCredentials encryptingCredentials { get; set; }
        private TokenValidationParameters validationParm { get; set; }
        private JwtSecurityTokenHandler tokenHandler { get; set; }
        public JWTUtil(IConfiguration config)
        {
            var signingKey = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:SigningKey").Value);
            var encryptKey = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:EncryptKey").Value);
            byte[] encryptKey32 = new byte[256 / 8];
            Array.Copy(encryptKey, encryptKey32, 256 / 8);
            validationParm = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(signingKey),
                TokenDecryptionKey = new SymmetricSecurityKey(encryptKey),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                RequireExpirationTime = false,
                ClockSkew = TimeSpan.Zero
            };
            signingCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey), 
                SecurityAlgorithms.HmacSha256Signature);
            encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptKey32), 
                SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

            tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap.Clear();
            tokenHandler.OutboundClaimTypeMap.Clear();
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
        public AuthResult GenerateJwtToken(string user, List<Claim> claims)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10), // for testing logics after expiry
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
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
