using FluentResults;
using MediatR;
using SharedServices.Core.Enums;

namespace SharedServices.Application.Features.Tenants.Commands
{
    public class TenantRegistrationCommand: IRequest<Result<long>>
    {
        public required string TenantName { get; set; }
        public required string TenantCode { get; set; }
        public required string Email { get; set; }
        public SubscriptionTier SubscriptionTier { get; set; }
        public string? MaxLocations { get; set; }
    }
}
