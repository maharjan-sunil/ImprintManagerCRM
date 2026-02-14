using FluentResults;
using MediatR;
using SharedServices.Application.Features.Users.Common.Models;
using SharedServices.Application.Features.Users.Dtos;

namespace SharedServices.Application.Features.Users.Commands
{
    public class UpdateUserCommand: UserBase, IRequest<Result<UserResponseDto>>
    {
        public required string Id { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}
