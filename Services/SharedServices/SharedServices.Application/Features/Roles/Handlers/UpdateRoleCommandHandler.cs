using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Features.Roles.Dtos;
using SharedServices.Application.Interfaces;
using SharedServices.Core.Entities;
using System.Data;

namespace SharedServices.Application.Features.Roles.Handlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Result<RoleResponseDto>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public UpdateRoleCommandHandler(RoleManager<Role> roleManager, ISharedServiceDbContext sharedServiceDbContext)
        {
            _roleManager = roleManager;
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<RoleResponseDto>> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingRole = await _roleManager.FindByIdAsync(command.Id);

                if (existingRole is null)
                    return Result.Fail(ResultHelper.WithErrorCode("Role not found.", ErrorCode.NotFound));

                var duplicateRole = await _roleManager.FindByNameAsync(command.Name);
                if (duplicateRole != null && duplicateRole.Id != existingRole.Id)
                    return Result.Fail(ResultHelper.WithErrorCode("Role name already exists.", ErrorCode.ValidationFailed));

                var validPermissions = await _sharedServiceDbContext.Permissions.Where(p => command.PermissionIds!.Contains(p.PermissionId)).ToListAsync(cancellationToken);

                if (validPermissions.Count != command.PermissionIds!.Count)
                    return Result.Fail(ResultHelper.WithErrorCode("One or more permissions are invalid.", ErrorCode.ValidationFailed));

                existingRole.Name = command.Name;
                existingRole.Description = command.Description;
                existingRole.IsActive  = command.IsActive;

                var result = await _roleManager.UpdateAsync(existingRole);
                if (!result.Succeeded)
                    Result.Fail(ResultHelper.WithErrorCode("Failed to update role.", ErrorCode.InternalError));

                var currentRolePermissions = await _sharedServiceDbContext.RolePermissions.Where(rp => rp.RoleId == existingRole.Id).ToListAsync(cancellationToken);

                var currentPermissionIds = currentRolePermissions.Select(rp => rp.PermissionId).ToList();

                var permissionsToAdd = command.PermissionIds.Except(currentPermissionIds).ToList();
                var permissionsToRemove = currentPermissionIds.Except(command.PermissionIds).ToList();

                if (permissionsToRemove.Any())
                {
                    var toRemove = currentRolePermissions.Where(rp => permissionsToRemove.Contains(rp.PermissionId)).ToList();
                    _sharedServiceDbContext.RolePermissions.RemoveRange(toRemove);
                }

                if (permissionsToAdd.Any())
                {
                    var toAdd = permissionsToAdd.Select(pid => new RolePermission
                    {
                        RoleId = existingRole.Id,
                        PermissionId = pid
                    });
                    await _sharedServiceDbContext.RolePermissions.AddRangeAsync(toAdd, cancellationToken);
                }

                await _sharedServiceDbContext.SaveChangesAsync(cancellationToken);

                return Result.Ok(new RoleResponseDto { Id = existingRole.Id, Name = existingRole.Name, Description = existingRole.Description}).WithSuccess("Role update successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to update role.", ErrorCode.InternalError));
        }
    }
}
