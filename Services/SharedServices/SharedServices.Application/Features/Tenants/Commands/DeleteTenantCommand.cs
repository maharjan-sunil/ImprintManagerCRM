using FluentResults;
using MediatR;

namespace SharedServices.Application.Features.Tenants.Commands
{
    public class DeleteTenantCommand: IRequest<Result<bool>>
    {
        public long TenantId { get; set; }
    }
}
