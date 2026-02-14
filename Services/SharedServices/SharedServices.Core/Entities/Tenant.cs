using Shared.Entities;
using SharedServices.Core.Enums;

namespace SharedServices.Core.Entities
{
    public class Tenant: SoftDeletableEntity
    {
        public long TenantId { get; set; }
        public required string TenantName { get; set; }
        public required string TenantCode { get; set; }
        public required string Email { get; set; }
        public SubscriptionTier SubscriptionTier { get; set; }
        public string? MaxLocations { get; set; }
        public bool IsActive { get; set; }
    }
}
