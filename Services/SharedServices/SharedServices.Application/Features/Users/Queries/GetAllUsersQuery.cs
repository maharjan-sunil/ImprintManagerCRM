using FluentResults;
using MediatR;
using SharedServices.Application.Features.Users.Dtos;

namespace SharedServices.Application.Features.Users.Queries
{
    public class GetAllUsersQuery: IRequest<Result<List<UserResponseDto>>>
    {

    }
}