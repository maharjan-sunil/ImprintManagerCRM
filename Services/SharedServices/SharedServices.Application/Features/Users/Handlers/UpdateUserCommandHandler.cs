using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Users.Commands;
using SharedServices.Application.Features.Users.Dtos;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Users.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result<UserResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UpdateUserCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<UserResponseDto>> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByIdAsync(command.Id);

                if (existingUser is null)
                    return Result.Fail(ResultHelper.WithErrorCode("User not found.", ErrorCode.NotFound));

                var duplicateUser = await _userManager.FindByEmailAsync(command.Email);
                if (duplicateUser != null && duplicateUser.Id != existingUser.Id)
                    return Result.Fail(ResultHelper.WithErrorCode($"Email '{command.Email}' is already registered.", ErrorCode.ValidationFailed));

                var validRoles = await _roleManager.Roles.Where(r => command.RoleIds!.Contains(r.Id)).ToListAsync(cancellationToken);

                if (validRoles.Count != command.RoleIds!.Count)
                    return Result.Fail(ResultHelper.WithErrorCode("One or more roles are invalid.", ErrorCode.ValidationFailed));

                existingUser.TenantId = command.TenantId;
                existingUser.FirstName = command.FirstName;
                existingUser.LastName = command.LastName;
                existingUser.IsActive = command.IsActive;

                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                    Result.Fail(ResultHelper.WithErrorCode("Failed to update user.", ErrorCode.InternalError));

                var currentRoles = await _userManager.GetRolesAsync(existingUser);
                var requestedRoleNames = validRoles.Select(r => r.Name!).ToList();

                var rolesToAdd = requestedRoleNames.Except(currentRoles).ToList();
                var rolesToRemove = currentRoles.Except(requestedRoleNames).ToList();

                if (rolesToRemove.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(existingUser, rolesToRemove);
                    if (!removeResult.Succeeded)
                        return Result.Fail(ResultHelper.WithErrorCode("Failed to update user.", ErrorCode.InternalError));
                }

                if (rolesToAdd.Any())
                {
                    var addResult = await _userManager.AddToRolesAsync(existingUser, rolesToAdd);
                    if (!addResult.Succeeded)
                        return Result.Fail(ResultHelper.WithErrorCode("Failed to update user.", ErrorCode.InternalError));
                }

                return Result.Ok(new UserResponseDto { Id = existingUser.Id, TenantId = existingUser.TenantId, Email = existingUser.Email!, FirstName = existingUser.FirstName, LastName = existingUser.LastName, IsActive = command.IsActive, RoleIds = rolesToAdd }).WithSuccess("User update successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to update user.", ErrorCode.InternalError));
        }
    }
}
