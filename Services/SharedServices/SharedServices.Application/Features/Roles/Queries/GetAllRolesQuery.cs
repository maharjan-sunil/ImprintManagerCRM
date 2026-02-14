using FluentResults;
using MediatR;
using SharedServices.Application.Features.Roles.Dtos;

namespace SharedServices.Application.Features.Roles.Queries
{
    public class GetAllRolesQuery: IRequest<Result<List<RoleResponseDto>>>
    {

    }
}
