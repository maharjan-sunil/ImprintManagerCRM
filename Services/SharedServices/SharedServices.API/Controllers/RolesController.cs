using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;
using SharedServices.Application.Features.Roles.Commands;
using SharedServices.Application.Features.Roles.Dtos;
using SharedServices.Application.Features.Roles.Queries;
using static ApiAbstractions.Extensions.ApiResponseExtensions;

namespace SharedServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly ISender _sender;

        public RolesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllRolesQuery(), cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetRoleByIdQuery { Id = id }, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromBody] UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<RoleResponseDto>.FailResponse(new List<string> { "Id in route and body must match" }));

            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteRoleCommand { Id = id }, cancellationToken);
            return result.ToActionResult();
        }
    }
}
