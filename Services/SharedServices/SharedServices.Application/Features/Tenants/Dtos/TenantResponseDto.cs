using SharedServices.Application.Features.Tenants.Common.Models;

namespace SharedServices.Application.Features.Tenants.Dtos
{
    public class TenantResponseDto: TenantBase
    {
        public long TenantId { get; set; }
    }
}
