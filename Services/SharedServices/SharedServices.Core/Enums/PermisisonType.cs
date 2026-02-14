namespace SharedServices.Core.Enums
{
    public enum PermissionType
    {
        #region General
        ViewDashboard = 101,
        #endregion

        #region User
        ViewUsers = 201,
        CreateUser = 202,
        UpdateUser = 203,
        DeleteUser = 204,
        #endregion

        #region Role
        ViewRoles = 301,
        CreateRole = 302,
        UpdateRole = 303,
        DeleteRole = 304,
        #endregion

        #region Permission
        ViewPermissions = 401,
        #endregion

        #region Tenant
        ViewTenants = 501,
        CreateTenant = 502,
        UpdateTenant = 503,
        DeleteTenant = 504,
        #endregion
    }
}
