using FluentResults;
using MediatR;

namespace SharedServices.Application.Features.Roles.Commands
{
    public class DeleteRoleCommand: IRequest<Result<bool>>
    {
        public required string Id { get; set; }
    }
}
