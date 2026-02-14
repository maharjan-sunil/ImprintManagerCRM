using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Tenants.Commands;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Tenants.Handlers
{
    public class DeleteTenantCommandHandler : IRequestHandler<DeleteTenantCommand, Result<bool>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public DeleteTenantCommandHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<bool>> Handle(DeleteTenantCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingTenant = await _sharedServiceDbContext.Tenants.AsNoTracking().Where(x => x.TenantId == command.TenantId).FirstOrDefaultAsync();
                if (existingTenant is null)
                    return Result.Fail(ResultHelper.WithErrorCode("Tenant not found.", ErrorCode.NotFound));

                _sharedServiceDbContext.Tenants.Remove(existingTenant);
                await _sharedServiceDbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(true).WithSuccess("Tenant deleted successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to delete tenant.", ErrorCode.InternalError));
        }
    }
}
