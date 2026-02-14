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
    public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, Result<List<RoleResponseDto>>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetAllRolesQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<List<RoleResponseDto>>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _sharedServiceDbContext.Roles.Where(x=> x.Name != "SuperAdmin")
                            .GroupJoin(
                                _sharedServiceDbContext.RolePermissions,
                                role => role.Id,
                                rolePermission => rolePermission.RoleId,
                                (role, rolePermissions) => new RoleResponseDto
                                {
                                    Id = role.Id,
                                    Name = role.Name!,
                                    Description = role.Description,
                                    IsActive = role.IsActive,
                                    PermissionIds = rolePermissions.Select(rp => rp.PermissionId).ToList()
                                }).AsNoTracking().ToListAsync(cancellationToken);

                return Result.Ok(roles).WithSuccess("Roles retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get roles.", ErrorCode.InternalError));
        }
    }
}
