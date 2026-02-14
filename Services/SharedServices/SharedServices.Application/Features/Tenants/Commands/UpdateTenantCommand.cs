using FluentResults;
using MediatR;
using SharedServices.Application.Features.Tenants.Common.Models;
using SharedServices.Application.Features.Tenants.Dtos;

namespace SharedServices.Application.Features.Tenants.Commands
{
    public class UpdateTenantCommand: TenantBase, IRequest<Result<TenantResponseDto>>
    {
        public long TenantId { get; set; }
    }
}
