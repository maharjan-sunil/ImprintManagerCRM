using Shared.Entities;

namespace SharedServices.Core.Entities
{
    public class Permission: SoftDeletableEntity
    {
        public long PermissionId { get; set; }
        public required string PermissionName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
        public bool IsActive { get; set; }
    }
}
