using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedServices.Application.Common.Models;
using SharedServices.Application.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SharedServices.Infrastructure.Authentication
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(UserTokenInfo userTokenInfo)
        {
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]!);
            var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]!);

            var claims = GetClaims(userTokenInfo);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(securityToken);
        }

        public IEnumerable<Claim> GetClaims(UserTokenInfo userTokenInfo)
        {
            var claims = new List<Claim>
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, userTokenInfo.UserId),
                            new Claim(JwtRegisteredClaimNames.Name, userTokenInfo.UserName),
                            new Claim("tenantId", userTokenInfo.TenantId.ToString())
                        };

            foreach (var role in userTokenInfo.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (var permission in userTokenInfo.Permissions)
            {
                claims.Add(new Claim("permissions", permission.ToString()));
            }

            return claims;
        }
    }
}
