using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Permissions.Dtos;
using SharedServices.Application.Features.Permissions.Queries;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Permissions.Handlers
{
    public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionsQuery, Result<List<PermissionResponseDto>>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetAllPermissionQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<List<PermissionResponseDto>>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var permissions = await _sharedServiceDbContext.Permissions.AsNoTracking().OrderBy(r => r.PermissionName).Select(r => new PermissionResponseDto
                {
                    PermissionId = r.PermissionId,
                    PermissionName = r.PermissionName,
                    Description = r.Description,
                    Category = r.Category,
                    IsActive = r.IsActive,
                }).ToListAsync(cancellationToken);

                return Result.Ok(permissions).WithSuccess("Permissions retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get permissions.", ErrorCode.InternalError));
        }
    }
}
