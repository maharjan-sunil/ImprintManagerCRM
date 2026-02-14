using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedServices.Application.Interfaces;
using SharedServices.Core.Entities;

namespace SharedServices.Infrastructure.Persistence
{
    public class SharedServiceDbContext : IdentityDbContext<User, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>, ISharedServiceDbContext
    {
        public SharedServiceDbContext(DbContextOptions<SharedServiceDbContext> options)
            : base(options) { }

        public DbSet<Permission> Permissions => Set<Permission>();
        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        public DbSet<Tenant> Tenants => Set<Tenant>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<UserRole>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<User>(b =>
            {
                b.HasKey(u => u.Id);

                b.Property(u => u.UserName).HasMaxLength(256);
                b.Property(u => u.NormalizedUserName).HasMaxLength(256);
                b.Property(u => u.Email).HasMaxLength(256);
                b.Property(u => u.NormalizedEmail).HasMaxLength(256);

                b.HasIndex(u => u.NormalizedUserName).IsUnique();
                b.HasIndex(u => u.NormalizedEmail);

                b.HasMany(u => u.UserRoles)
                 .WithOne(ur => ur.User!)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.HasKey(r => r.Id);

                b.Property(r => r.Name).HasMaxLength(256);
                b.Property(r => r.NormalizedName).HasMaxLength(256);

                b.HasIndex(r => r.NormalizedName).IsUnique();

                b.HasMany(r => r.UserRoles)
                 .WithOne(ur => ur.Role!)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();
            });

            builder.Entity<UserRole>(b =>
            {
                b.HasKey(ur => new { ur.UserId, ur.RoleId });
            });
        }
    }
}
