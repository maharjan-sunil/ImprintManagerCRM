using FluentResults;
using MediatR;
using Shared.Common.Models;
using SharedServices.Application.Features.Authentication.Dtos;

namespace SharedServices.Application.Features.Authentication.Commands
{
    public class LoginUserCommand: IRequest<Result<LoginUserResponseDto>>
    {
        public required string Email { get; init; }
        public required string Password { get; init; }

    }
}
