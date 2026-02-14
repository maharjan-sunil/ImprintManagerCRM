using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Permissions.Dtos;
using SharedServices.Application.Features.Tenants.Dtos;
using SharedServices.Application.Features.Tenants.Queries;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Tenants.Handlers
{
    public class GetAllTenantsQueryHandler : IRequestHandler<GetAllTenantsQuery, Result<List<TenantResponseDto>>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetAllTenantsQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<List<TenantResponseDto>>> Handle(GetAllTenantsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenants = await _sharedServiceDbContext.Tenants.AsNoTracking().OrderBy(r => r.TenantName).Select(r => new TenantResponseDto
                {
                    TenantId = r.TenantId,
                    TenantName = r.TenantName,
                    TenantCode = r.TenantCode,
                    Email = r.Email,
                    SubscriptionTier = r.SubscriptionTier,
                    MaxLocations = r.MaxLocations,
                    IsActive = r.IsActive,
                }).ToListAsync(cancellationToken);

                return Result.Ok(tenants).WithSuccess("Tenants retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get tenants.", ErrorCode.InternalError));
        }
    }
}
