using SharedServices.Application.Interfaces;
using SharedServices.Core.Enums;
using System.Security.Claims;

namespace SharedServices.Infrastructure.Authorization
{
    public class PermissionService : IPermissionService
    {
        public bool HasPermission(ClaimsPrincipal user, PermissionType permission)
        {
            if (user == null) return false;

            // SuperAdmin bypass
            if (user.IsInRole("SuperAdmin") ||
                user.Claims.Any(c => c.Type == "IsSuperAdmin" && c.Value == "true"))
            {
                return true;
            }

            var permissions = user.Claims
                .Where(c => c.Type == "permissions")
                .SelectMany(c => c.Value.Split(',', StringSplitOptions.RemoveEmptyEntries))
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            return permissions.Contains(permission.ToString());
        }
    }
}
