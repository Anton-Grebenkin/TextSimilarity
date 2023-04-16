using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using TextSimilarity.API.Common.DataAccess.Entities;

namespace TextSimilarity.API.Common.Security.Authentication
{
    public interface IJWTService
    {
        public string GenerateToken(long userId);
        public bool ValidateToken(string token, out long? userId);
    }
    public class JWTService : IJWTService
    {
        private const string USER_ID_CLAIM_TYPE = "userId";
        private readonly JWTSettings _settings;
        public JWTService(IOptions<JWTSettings> options)
        {
            _settings = options.Value;
        }
        public string GenerateToken(long userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(USER_ID_CLAIM_TYPE, userId.ToString()) }),
                Expires = DateTime.UtcNow.AddSeconds(_settings.TokenLifetimeInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string token, out long? userId)
        {
            userId = null;

            if (token == null)
                return false;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Secret);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                userId = long.Parse(jwtToken.Claims.First(x => x.Type == USER_ID_CLAIM_TYPE).Value);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
