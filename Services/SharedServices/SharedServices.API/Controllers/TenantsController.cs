using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;
using SharedServices.Application.Features.Tenants.Commands;
using SharedServices.Application.Features.Tenants.Dtos;
using SharedServices.Application.Features.Tenants.Queries;
using static ApiAbstractions.Extensions.ApiResponseExtensions;

namespace SharedServices.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly ISender _sender;

        public TenantsController(ISender sender)
        {
            _sender = sender;
        }

        //for tenant registration
        [AllowAnonymous]
        [HttpPost("registration")]
        public async Task<IActionResult> TenantRegistration(TenantRegistrationCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet]
        public async Task<IActionResult> GetTenants(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllTenantsQuery(), cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTenantById(long id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetTenantByIdQuery { TenantId = id }, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTenant([FromBody] CreateTenantCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTenant(long id, [FromBody] UpdateTenantCommand command, CancellationToken cancellationToken)
        {
            if (id != command.TenantId)
                return BadRequest(ApiResponse<TenantResponseDto>.FailResponse(new List<string> { "Id in route and body must match" }));

            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTenant(long id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteTenantCommand { TenantId = id }, cancellationToken);
            return result.ToActionResult();
        }
    }
}
