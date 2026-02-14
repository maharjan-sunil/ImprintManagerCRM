using FluentResults;
using MediatR;
using SharedServices.Application.Features.Tenants.Dtos;

namespace SharedServices.Application.Features.Tenants.Queries
{
    public class GetAllTenantsQuery: IRequest<Result<List<TenantResponseDto>>>
    {

    }
}
