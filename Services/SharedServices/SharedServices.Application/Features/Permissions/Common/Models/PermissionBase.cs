namespace SharedServices.Application.Features.Permissions.Common.Models
{
    public class PermissionBase
    {
        public required string PermissionName { get; set; }
        public string? Description { get; set; }
        public string? Category { get; set; }
    }
}
