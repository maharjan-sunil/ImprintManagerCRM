using Microsoft.AspNetCore.Identity;

namespace SharedServices.Core.Entities
{
    public class UserRole: IdentityUserRole<string>
    {
        public long LocationId { get; set; }
        public bool IsActive { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public User? User { get; set; }
        public Role? Role { get; set; }
    }
}
