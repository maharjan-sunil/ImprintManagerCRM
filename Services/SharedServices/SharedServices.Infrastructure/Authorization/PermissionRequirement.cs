using Microsoft.AspNetCore.Authorization;
using SharedServices.Core.Enums;

namespace SharedServices.Infrastructure.Authorization
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionType Permission { get; }

        public PermissionRequirement(PermissionType permission)
        {
            Permission = permission;
        }
    }
}
