using FluentResults;
using MediatR;
using SharedServices.Application.Features.Users.Common.Models;

namespace SharedServices.Application.Features.Users.Commands
{
    public class CreateUserCommand: UserBase, IRequest<Result<string>>
    {
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
