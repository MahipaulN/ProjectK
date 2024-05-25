using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Microsoft.IdentityModel.Tokens;

namespace PROJECT_K.Repository
{
    public class JwtDecoder
    {
        public static string DecodeJwtToken(string token, string secretKey, string requiredclaim)
        {
            // Create token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                string roleValue="";
                // Validate token and extract claims
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claims = jwtToken.Claims;
                foreach (var claim in claims)
                {
                    // Console.WriteLine($"Claim Type: {claim.Type}, Claim Value: {claim.Value}");
                    if(claim.Type==requiredclaim){
                        roleValue = claim.Value;
                    }
                }
                return roleValue;
            }
            catch (SecurityTokenException e)
            {
                return $"Token validation failed: {e.Message}";
            }
        }
    }
}