using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Features.Roles.Dtos;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Roles.Handlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, Result<bool>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public DeleteRoleCommandHandler(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DeleteRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(command.Id);
                if (role is null)
                    return Result.Fail(ResultHelper.WithErrorCode("Role not found.", ErrorCode.NotFound));

                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name!);
                if (usersInRole.Any())
                    return Result.Fail(ResultHelper.WithErrorCode("Role cannot be deleted because it is assigned to one or more users.", ErrorCode.Conflict));

                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                    Result.Fail(ResultHelper.WithErrorCode("Failed to delete role.", ErrorCode.InternalError));

                return Result.Ok(true).WithSuccess("Role deleted successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to delete role.", ErrorCode.InternalError));
        }
    }
}
