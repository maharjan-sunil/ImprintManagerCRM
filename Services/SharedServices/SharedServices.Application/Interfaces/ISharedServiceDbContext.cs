using Microsoft.EntityFrameworkCore;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Interfaces
{
    public interface ISharedServiceDbContext
    {
        DbSet<User> Users { get; }
        DbSet<Role> Roles { get; }
        DbSet<UserRole> UserRoles { get; }
        DbSet<Permission> Permissions { get; }
        DbSet<RolePermission> RolePermissions { get; }
        DbSet<Tenant> Tenants { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
    }
}
