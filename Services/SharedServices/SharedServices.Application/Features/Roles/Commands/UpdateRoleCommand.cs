using FluentResults;
using MediatR;
using SharedServices.Application.Features.Roles.Common.Models;
using SharedServices.Application.Features.Roles.Dtos;

namespace SharedServices.Application.Features.Roles.Commands
{
    public class UpdateRoleCommand : RoleBase, IRequest<Result<RoleResponseDto>>
    {
        public required string Id { get; set; }
    }
}
