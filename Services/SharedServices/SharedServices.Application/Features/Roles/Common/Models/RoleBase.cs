namespace SharedServices.Application.Features.Roles.Common.Models
{
    public class RoleBase
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public List<long>? PermissionIds { get; set; }
        public bool IsActive { get; set; }
    }
}
