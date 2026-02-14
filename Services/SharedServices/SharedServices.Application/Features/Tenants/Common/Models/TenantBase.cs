using SharedServices.Core.Enums;

namespace SharedServices.Application.Features.Tenants.Common.Models
{
    public class TenantBase
    {
        public required string TenantName { get; set; }
        public required string TenantCode { get; set; }
        public required string Email { get; set; }
        public SubscriptionTier SubscriptionTier { get; set; }
        public string? MaxLocations { get; set; }
        public bool IsActive { get; set; }
    }
}
