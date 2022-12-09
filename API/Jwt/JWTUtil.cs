using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Common;
using Business;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;

namespace API {
    public class JWTUtil : IJWTUtil {
        private SigningCredentials signingCredentials { get; set; }
        private EncryptingCredentials encryptingCredentials { get; set; }
        private TokenValidationParameters validationParm { get; set; }
        private JwtSecurityTokenHandler tokenHandler { get; set; }
        private readonly ILogger<JWTUtil> _logger;
        public JWTUtil(IConfiguration config, ILogger<JWTUtil> logger) {
            _logger = logger;
            var signingKey = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:SigningKey").Value);
            var encryptKey = Encoding.ASCII.GetBytes(config.GetSection("JwtConfig:EncryptKey").Value);
            validationParm = new TokenValidationParameters {
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
            encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptKey),
                SecurityAlgorithms.Aes256KW, SecurityAlgorithms.Aes256CbcHmacSha512);

            tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap.Clear();
            tokenHandler.OutboundClaimTypeMap.Clear();
        }

        public bool ValidateToken(HttpRequest request, out JwtSecurityToken jwtToken, out string token) {
            StringValues tokens = request.Cookies["X-UserRoles"];
            if (tokens.Any()) {
                token = tokens[0];
                if (string.IsNullOrEmpty(token)) {
                    throw new JWTException("token is null");
                }
                try {
                    tokenHandler.ValidateToken(token, validationParm,
                            out SecurityToken validatedToken);
                    jwtToken = (JwtSecurityToken)validatedToken;

                    if (jwtToken.ValidTo.Subtract(DateTime.UtcNow) < new TimeSpan(0, 0, 10)) {
                        return false;
                    }
                } catch (SecurityTokenExpiredException) {
                    jwtToken = null;
                    return false;
                }
            } else {
                token = null;
                jwtToken = null;
                return false;
            }
            return true;
        }
        public AuthResult GenerateJwtToken(string user, List<Claim> claims) {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(10), // for testing logics after expiry
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            if (jwtToken.Length > (4096 - 13)) { //limit - cookie key
                throw new CookieSizeExceedLimitException();
            }
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
            return new AuthResult() {
                Token = jwtToken,
                Success = true,
                RefreshToken = refreshToken.Token
            };
        }
        private string RandomString(int length) {
            var random = new Random();
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(x => x[random.Next(x.Length)]).ToArray());
        }
    }
}
