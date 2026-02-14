using FluentResults;
using MediatR;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Tenants.Commands;
using SharedServices.Application.Features.Tenants.Dtos;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Tenants.Handlers
{
    public class UpdateTenantCommandHandler : IRequestHandler<UpdateTenantCommand, Result<TenantResponseDto>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public UpdateTenantCommandHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<TenantResponseDto>> Handle(UpdateTenantCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingTenant = await _sharedServiceDbContext.Tenants.FindAsync(command.TenantId);

                if (existingTenant is null)
                    return Result.Fail(ResultHelper.WithErrorCode("Tenant not found.", ErrorCode.NotFound));

                existingTenant.TenantName = command.TenantName;
                existingTenant.TenantCode = command.TenantCode;
                existingTenant.SubscriptionTier = command.SubscriptionTier;
                existingTenant.MaxLocations = command.MaxLocations;
                existingTenant.IsActive = command.IsActive;

                await _sharedServiceDbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(new TenantResponseDto { TenantId = existingTenant.TenantId, TenantName = existingTenant.TenantName, TenantCode = existingTenant.TenantCode, Email = command.Email, SubscriptionTier = existingTenant.SubscriptionTier, MaxLocations = existingTenant.MaxLocations, IsActive = command.IsActive }).WithSuccess("Tenant update successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to update tenant.", ErrorCode.InternalError));
        }
    }
}
