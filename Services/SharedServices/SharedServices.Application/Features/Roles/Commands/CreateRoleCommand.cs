using FluentResults;
using MediatR;
using SharedServices.Application.Features.Roles.Common.Models;

namespace SharedServices.Application.Features.Roles.Commands
{
    public class CreateRoleCommand: RoleBase, IRequest<Result<string>>
    {

    }
}
