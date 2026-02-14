using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using Shared.Common.Models;
using SharedServices.Application.Common.Models;
using SharedServices.Application.Features.Authentication.Commands;
using SharedServices.Application.Features.Authentication.Dtos;
using SharedServices.Application.Interfaces;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Authentication.Handlers
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResponseDto>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public LoginUserCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator, ISharedServiceDbContext sharedServiceDbContext)
        {
            _userManager = userManager;
            _jwtTokenGenerator = jwtTokenGenerator;
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<LoginUserResponseDto>> Handle(LoginUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(command.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, command.Password))
                {
                    return Result.Fail(ResultHelper.WithErrorCode("Invalid credentials. Please try again.", ErrorCode.InvalidCredentials));
                }

                var roles = await _userManager.GetRolesAsync(user);


                List<string> permissions = new List<string>();

                if (roles.Contains("SuperAdmin"))
                {
                    permissions = new List<string> { "*" };
                }
                else
                {
                    permissions = await _sharedServiceDbContext.Roles.Where(r => roles.Contains(r.Name!)).SelectMany(r => r.RolePermissions.Select(rp => rp.Permission.PermissionName)).Distinct().ToListAsync(cancellationToken);
                }

                UserTokenInfo userTokenInfo = new UserTokenInfo
                {
                    UserId = user.Id,
                    UserName = user.UserName!,
                    Roles = roles.ToList(),
                    TenantId = user.TenantId,
                    Permissions = permissions
                };
                var token = _jwtTokenGenerator.GenerateToken(userTokenInfo);

                LoginUserResponseDto response = new LoginUserResponseDto
                {
                    //UserId = user.Id,
                    //Username = user.UserName!,
                    //Role = roles.FirstOrDefault()!,
                    //TenantId = user.TenantId,
                    //Permissions = permissions,
                    AccessToken = token
                };
                return Result.Ok(response).WithSuccess("Login successfully.");

            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Invalid credentials. Please try again.", ErrorCode.InvalidCredentials));
        }
    }
}
