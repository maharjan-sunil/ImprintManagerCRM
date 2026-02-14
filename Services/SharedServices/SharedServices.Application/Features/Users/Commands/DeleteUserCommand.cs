using FluentResults;
using MediatR;

namespace SharedServices.Application.Features.Users.Commands
{
    public class DeleteUserCommand: IRequest<Result<bool>>
    {
        public required string Id { get; set; }
    }
}
