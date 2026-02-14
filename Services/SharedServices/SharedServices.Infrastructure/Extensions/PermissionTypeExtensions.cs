using SharedServices.Core.Enums;

namespace SharedServices.Infrastructure.Extensions
{
    public static class PermissionTypeExtensions
    {
        public static string GetCategory(this PermissionType p)
        {
            if ((int)p >= 200 && (int)p < 300) return PermissionCategory.UserManagement.ToString();
            if ((int)p >= 300 && (int)p < 400) return PermissionCategory.RoleManagement.ToString();
            if ((int)p >= 400 && (int)p < 500) return PermissionCategory.PermissionManagement.ToString();
            if ((int)p >= 500 && (int)p < 600) return PermissionCategory.TenantManagement.ToString();
            return "General";
        }
    }
}
