using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Features.Users.Commands;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Users.Handlers
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Result<bool>>
    {
        private readonly UserManager<User> _userManager;

        public DeleteUserCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Result<bool>> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(command.Id);
                if (user is null)
                    return Result.Fail(ResultHelper.WithErrorCode("User not found.", ErrorCode.NotFound));

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                    Result.Fail(ResultHelper.WithErrorCode("Failed to delete user.", ErrorCode.InternalError));

                return Result.Ok(true).WithSuccess("User deleted successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to delete user.", ErrorCode.InternalError));
        }
    }
}
