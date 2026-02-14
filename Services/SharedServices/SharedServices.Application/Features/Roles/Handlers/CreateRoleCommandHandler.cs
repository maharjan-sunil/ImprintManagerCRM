using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Interfaces;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Roles.Handlers
{
    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Result<string>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public CreateRoleCommandHandler(RoleManager<Role> roleManager, ISharedServiceDbContext sharedServiceDbContext)
        {
            _roleManager = roleManager;
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<string>> Handle(CreateRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (await _roleManager.RoleExistsAsync(command.Name))
                    Result.Fail(ResultHelper.WithErrorCode("Role already exists.", ErrorCode.ValidationFailed));

                var validPermissions = await _sharedServiceDbContext.Permissions.Where(p => command.PermissionIds!.Contains(p.PermissionId)).ToListAsync(cancellationToken);

                if (validPermissions.Count != command.PermissionIds!.Count)
                    return Result.Fail(ResultHelper.WithErrorCode("One or more permissions are invalid.", ErrorCode.ValidationFailed));

                var role = new Role
                {
                    Name = command.Name,
                    Description = command.Description,
                    IsActive = true
                };

                var result = await _roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                        Result.Fail(ResultHelper.WithErrorCode("Failed to create role.", ErrorCode.InternalError));

                var rolePermissions = command.PermissionIds.Select(pid => new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = pid
                });

                await _sharedServiceDbContext.RolePermissions.AddRangeAsync(rolePermissions, cancellationToken);
                await _sharedServiceDbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(role.Id).WithSuccess("Role created successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to create role.", ErrorCode.InternalError));
        }
    }
}
