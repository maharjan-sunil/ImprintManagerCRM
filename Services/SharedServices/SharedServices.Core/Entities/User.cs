using Microsoft.AspNetCore.Identity;

namespace SharedServices.Core.Entities
{
    public class User: IdentityUser<string>
    {
        public User()
        {
            Id = Guid.NewGuid().ToString();
        }

        public required long TenantId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public bool IsActive { get; set; }
        public bool IsAdmin { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new HashSet<UserRole>();
    }
}
