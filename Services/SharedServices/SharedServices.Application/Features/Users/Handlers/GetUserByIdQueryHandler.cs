using FluentResults;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using SharedServices.Application.Features.Users.Dtos;
using SharedServices.Application.Features.Users.Queries;
using SharedServices.Application.Interfaces;

namespace SharedServices.Application.Features.Users.Handlers
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, Result<UserResponseDto>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetUserByIdQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<UserResponseDto>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _sharedServiceDbContext.Users
                            .Where(x=>x.Id == request.Id)
                            .GroupJoin(
                                _sharedServiceDbContext.UserRoles,
                                user => user.Id,
                                userRole => userRole.UserId,
                                (user, userRoles) => new UserResponseDto
                                {
                                    Id = user.Id,
                                    TenantId = user.TenantId,
                                    FirstName = user.FirstName,
                                    LastName = user.LastName,
                                    Email = user.Email!,
                                    IsActive = user.IsActive,
                                    RoleIds = userRoles.Select(ur => ur.RoleId).ToList()
                                }).AsNoTracking().FirstOrDefaultAsync(cancellationToken);

                if (user is null)
                {
                    return Result.Fail(ResultHelper.WithErrorCode($"User of id: {request.Id} not found", ErrorCode.NotFound));
                }

                return Result.Ok(user).WithSuccess("User retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get user.", ErrorCode.InternalError));
        }
    }
}
