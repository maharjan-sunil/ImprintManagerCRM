using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Tenants.Dtos;
using SharedServices.Application.Features.Tenants.Queries;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Tenants.Handlers
{
    public class GetTenantByIdQueryHandler : IRequestHandler<GetTenantByIdQuery, Result<TenantResponseDto>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetTenantByIdQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<TenantResponseDto>> Handle(GetTenantByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = await _sharedServiceDbContext.Tenants
                            .Where(x => x.TenantId == request.TenantId).AsNoTracking().Select(r => new TenantResponseDto
                            {
                                TenantId = r.TenantId,
                                TenantName = r.TenantName,
                                TenantCode = r.TenantCode,
                                Email = r.Email,
                                SubscriptionTier = r.SubscriptionTier,
                                MaxLocations = r.MaxLocations,
                                IsActive = r.IsActive,
                            }).FirstOrDefaultAsync(cancellationToken);

                if (tenant is null)
                {
                    return Result.Fail(ResultHelper.WithErrorCode($"Tenant of id: {request.TenantId} not found", ErrorCode.NotFound));
                }

                return Result.Ok(tenant).WithSuccess("Tenant retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get tenant.", ErrorCode.InternalError));
        }
    }
}
