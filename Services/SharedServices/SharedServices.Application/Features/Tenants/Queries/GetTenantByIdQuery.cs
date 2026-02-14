using FluentResults;
using MediatR;
using SharedServices.Application.Features.Tenants.Dtos;

namespace SharedServices.Application.Features.Tenants.Queries
{
    public class GetTenantByIdQuery: IRequest<Result<TenantResponseDto>>
    {
        public long TenantId { get; set; }
    }
}
