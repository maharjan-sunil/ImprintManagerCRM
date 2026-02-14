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
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, Result<List<UserResponseDto>>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;

        public GetAllUsersQueryHandler(ISharedServiceDbContext sharedServiceDbContext)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
        }

        public async Task<Result<List<UserResponseDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var Users = await _sharedServiceDbContext.Users.Where(x=> x.Email != "admin@imprintcrm.com")
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
                                }).AsNoTracking().ToListAsync(cancellationToken);

                return Result.Ok(Users).WithSuccess("Users retrieved successfully.");
            }
            catch (Exception ex)
            {

            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to get users.", ErrorCode.InternalError));
        }
    }
}
