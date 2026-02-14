using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedServices.Core.Entities;
using SharedServices.Core.Enums;
using static SharedServices.Infrastructure.Extensions.PermissionTypeExtensions;

namespace SharedServices.Infrastructure.Persistence
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            #region Initialization
            using var scope = serviceProvider.CreateScope();
            var _roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
            var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var _sharedServiceDbContext = scope.ServiceProvider.GetRequiredService<SharedServiceDbContext>();
            #endregion

            #region Seed Roles
            var tenantRole = await _roleManager.FindByNameAsync("Tenant");
            if (tenantRole is null)
            {
                tenantRole = new Role
                {
                    Name = "Tenant",
                    IsActive = true
                };
                await _roleManager.CreateAsync(tenantRole);
            }

            var superAdminRole = await _roleManager.FindByNameAsync("SuperAdmin");
            if (superAdminRole == null)
            {
                superAdminRole = new Role
                {
                    Name = "SuperAdmin",
                    IsActive = true
                };
                await _roleManager.CreateAsync(superAdminRole);
            }
            #endregion

            #region Seed Permissions
            //var allPermissions = new List<Permission>
            //{
            //    new Permission { PermissionName = PermissionType.ViewDashboard.ToString(), Category = PermissionCategory.General.ToString(), IsActive = true },

            //    new Permission { PermissionName = PermissionType.ViewUsers.ToString(), Category = PermissionCategory.UserManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.CreateUser.ToString(), Category = PermissionCategory.UserManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.UpdateUser.ToString(), Category = PermissionCategory.UserManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.DeleteUser.ToString(), Category = PermissionCategory.UserManagement.ToString(), IsActive = true },

            //    new Permission { PermissionName = PermissionType.ViewRoles.ToString(), Category = PermissionCategory.RoleManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.CreateRole.ToString(), Category = PermissionCategory.RoleManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.UpdateRole.ToString(), Category = PermissionCategory.RoleManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.DeleteRole.ToString(), Category = PermissionCategory.RoleManagement.ToString(), IsActive = true },

            //    new Permission { PermissionName = PermissionType.ViewPermissions.ToString(), Category = PermissionCategory.PermissionManagement.ToString(), IsActive = true },

            //    new Permission { PermissionName = PermissionType.ViewTenants.ToString(), Category = PermissionCategory.TenantManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.CreateTenant.ToString(), Category = PermissionCategory.TenantManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.UpdateTenant.ToString(), Category = PermissionCategory.TenantManagement.ToString(), IsActive = true },
            //    new Permission { PermissionName = PermissionType.DeleteTenant.ToString(), Category = PermissionCategory.TenantManagement.ToString(), IsActive = true }
            //};

            var allPermissions = Enum.GetValues<PermissionType>().Select(p => new Permission
            {
                PermissionName = p.ToString(),
                Category = p.GetCategory(),
                Description = $"Allows {p}",
                IsActive = true
            });

            foreach (var permission in allPermissions)
            {
                bool exists = await _sharedServiceDbContext.Permissions
                    .AnyAsync(p => p.PermissionName == permission.PermissionName);

                if (!exists)
                {
                    _sharedServiceDbContext.Permissions.Add(permission);
                }
            }

            await _sharedServiceDbContext.SaveChangesAsync();
            #endregion

            #region Seed Admin User
            var adminUser = await _userManager.FindByNameAsync("admin@imprintcrm.com");
            if (adminUser is null)
            {
                adminUser = new User
                {
                    UserName = "admin@imprintcrm.com",
                    Email = "admin@imprintcrm.com",
                    TenantId = 0,
                    FirstName = "System",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    IsAdmin = true,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "SuperAdmin");
                }
                else
                {
                    throw new Exception("Admin user seeding failed: " +
                                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            #endregion
        }
    }
}
