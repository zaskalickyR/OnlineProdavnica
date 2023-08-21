using ECommerce.Models.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Helpers
{
    public static class JwtManager
    {
        private static string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";
        public static string GetToken(User user, int expireMinutes = 1440)
        {
            // symmetric security key
            var symemtricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("email", user.Email),
                    new Claim("id", user.Id.ToString()),
                    new Claim("role", user.Role.ToString()),
                    new Claim("name", $"{user.FirstName} {user.LastName}"),
                }),

                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireMinutes)),

                SigningCredentials = new SigningCredentials(symemtricSecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtHander = new JwtSecurityTokenHandler();

            var token = jwtHander.CreateToken(tokenDescriptor);

            // return token

            return jwtHander.WriteToken(token);
        }

        public static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters
            {
                // what to validate
                RequireExpirationTime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                // setup validate data
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret))
            };
        }

        public static int? GetLoggedUserID(string parameter)
        {
            var identity = GetPrincipal(parameter)?.Identity as ClaimsIdentity;
            if (identity == null)
                return null;
            return Int32.Parse(identity.FindFirst("id")?.Value);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var symmetricKey = Convert.FromBase64String(Secret);

                var principal = tokenHandler.ValidateToken(token, GetTokenValidationParameters(), out _);

                return principal;
            }

            catch (Exception e)
            {
                return null;
            }
        }
    }
}
