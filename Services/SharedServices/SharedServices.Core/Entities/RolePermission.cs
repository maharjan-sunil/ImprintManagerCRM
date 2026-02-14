using Shared.Entities;

namespace SharedServices.Core.Entities
{
    public class RolePermission: SoftDeletableEntity
    {
        public long RolePermissionId { get; set; }
        public required string RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public long PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
        public bool IsGranted { get; set; }

    }
}
