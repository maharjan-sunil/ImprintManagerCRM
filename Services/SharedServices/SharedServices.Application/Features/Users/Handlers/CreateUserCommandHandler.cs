using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Features.Users.Commands;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Users.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<string>>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public CreateUserCommandHandler(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<string>> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(command.Email);
                if (existingUser is not null)
                    return Result.Fail(ResultHelper.WithErrorCode($"Email '{command.Email}' is already registered.", ErrorCode.ValidationFailed));

                var validRoles = await _roleManager.Roles.Where(r => command.RoleIds.Contains(r.Id)).ToListAsync(cancellationToken);

                if (validRoles.Count != command.RoleIds.Count)
                    return Result.Fail(ResultHelper.WithErrorCode("One or more roles are invalid.", ErrorCode.ValidationFailed));

                var user = new User
                {
                    TenantId = command.TenantId,
                    UserName = command.Email,
                    Email = command.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    IsActive = command.IsActive
                };

                var result = await _userManager.CreateAsync(user, command.Password);
                if (!result.Succeeded)
                    Result.Fail(ResultHelper.WithErrorCode("Failed to create user.", ErrorCode.InternalError));

                var roleNames = validRoles.Select(r => r.Name!).ToList();

                var addRolesResult = await _userManager.AddToRolesAsync(user, roleNames);
                if (!addRolesResult.Succeeded)
                    return Result.Fail(ResultHelper.WithErrorCode("Failed to create user.", ErrorCode.InternalError));

                return Result.Ok(user.Id).WithSuccess("User created successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to create user.", ErrorCode.InternalError));
        }
    }
}
