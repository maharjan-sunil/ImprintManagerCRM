using FluentResults;
using MediatR;
using SharedServices.Application.Features.Roles.Dtos;

namespace SharedServices.Application.Features.Roles.Queries
{
    public class GetRoleByIdQuery: IRequest<Result<RoleResponseDto>>
    {
        public required string Id { get; set; }
    }
}
