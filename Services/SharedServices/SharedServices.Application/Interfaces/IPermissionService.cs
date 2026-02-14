using SharedServices.Core.Enums;
using System.Security.Claims;

namespace SharedServices.Application.Interfaces
{
    public interface IPermissionService
    {
        bool HasPermission(ClaimsPrincipal user, PermissionType permission);
    }
}
