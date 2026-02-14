using FluentResults;
using MediatR;
using SharedServices.Application.Features.Permissions.Dtos;

namespace SharedServices.Application.Features.Permissions.Queries
{
    public class GetAllPermissionsQuery: IRequest<Result<List<PermissionResponseDto>>>
    {

    }
}
