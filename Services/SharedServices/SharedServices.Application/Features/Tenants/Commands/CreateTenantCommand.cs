using FluentResults;
using MediatR;
using SharedServices.Application.Features.Tenants.Common.Models;

namespace SharedServices.Application.Features.Tenants.Commands
{
    public class CreateTenantCommand: TenantBase, IRequest<Result<long>>
    {
       
    }
}
