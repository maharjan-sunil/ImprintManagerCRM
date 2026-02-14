namespace SharedServices.Application.Features.Users.Common.Models
{
    public class UserBase
    {
        public required long TenantId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required List<string> RoleIds { get; set; }
        public bool IsActive { get; set; }
    }
}
