using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Dtos;
using SharedServices.Application.Features.Roles.Queries;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Roles.Handlers
{
    public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, Result<RoleResponseDto>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetRoleByIdQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<RoleResponseDto>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _sharedServiceDbContext.Roles
                          .Where(r => r.Id == request.Id)
                          .GroupJoin(
                              _sharedServiceDbContext.RolePermissions,
                              role => role.Id,
                              rolepermission => rolepermission.RoleId,
                              (role, rolepermission) => new RoleResponseDto
                              {
                                  Id = role.Id,
                                  Name = role.Name!,
                                  Description = role.Description,
                                  IsActive = role.IsActive,
                                  PermissionIds = rolepermission.Select(rp => rp.PermissionId).ToList()
                              }).AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                if (role is null)
                {
                    return Result.Fail(ResultHelper.WithErrorCode($"Role of id: {request.Id} not found", ErrorCode.NotFound));
                }

                return Result.Ok(role).WithSuccess("Role retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get role.", ErrorCode.InternalError));
        }
    }
}
