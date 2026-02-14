using FluentResults;
using MediatR;
using SharedServices.Application.Features.Users.Dtos;

namespace SharedServices.Application.Features.Users.Queries
{
    public class GetUserByIdQuery: IRequest<Result<UserResponseDto>>
    {
        public required string Id { get; set; }
    }
}
