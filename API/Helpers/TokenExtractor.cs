using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Helpers
{
    public static class TokenExtractor
    {

        public static string GetUserId(IConfiguration _config, HttpContext httpContext)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var SecretKey = _config.GetValue<string>("JWT:SecretKey");
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var token = httpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];

            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;

            return userId;
        }
    }
}
