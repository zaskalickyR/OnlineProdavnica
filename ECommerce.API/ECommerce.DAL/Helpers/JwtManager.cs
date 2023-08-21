using ECommerce.DAL.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DAL.Helpers
{
    public static class JwtManager
    {
        private static string Secret = "b90ajAJiosjdASF93261a4d351e7gasd(0k0daj@Qjcf478ea8d312c763bb6caca";
        public static string GetToken(User user, int expireMinutes = 1440)
        {
            var issuer = "Issuer";
            var audience = "Audience";
            var key = Encoding.UTF8.GetBytes("b90ajAJiosjdASF93261a4d351e7gasd(0k0daj@Qjcf478ea8d312c763bb6caca");

            var signingCredentials = new SigningCredentials(
                                               new SymmetricSecurityKey(key)
       ,
                                               SecurityAlgorithms.HmacSha512Signature);

            var subject = new ClaimsIdentity(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("email", user.Email),
                new Claim("id", user.Id.ToString()),
                new Claim("role", user.Role.ToString()),
                new Claim("image", user.Image.ToString()),
                new Claim("name", $"{user.FirstName}"),
                new Claim("lastName", $"{user.LastName}"),
                new Claim("address", $"{user.Address}"),
            });

            var expires = DateTime.Now.AddMinutes(100);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = subject,
                Expires = expires,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = signingCredentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
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
