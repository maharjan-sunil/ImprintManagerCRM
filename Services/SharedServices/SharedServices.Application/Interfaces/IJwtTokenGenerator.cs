using SharedServices.Application.Common.Models;
using System.Security.Claims;

namespace SharedServices.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(UserTokenInfo userTokenInfo);
        IEnumerable<Claim> GetClaims(UserTokenInfo userTokenInfo);
    }
}
