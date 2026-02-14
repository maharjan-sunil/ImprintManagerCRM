using Microsoft.AspNetCore.Identity;

namespace SharedServices.Core.Entities
{
    public class Role: IdentityRole<string>
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string? Description { get; set; }
        public bool IsSystemRole { get; set; }
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
        public ICollection<RolePermission> RolePermissions { get; set; } = new HashSet<RolePermission>();
    }
}
