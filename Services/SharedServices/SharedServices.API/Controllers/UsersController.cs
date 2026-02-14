using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.Models;
using SharedServices.Application.Features.Users.Commands;
using SharedServices.Application.Features.Users.Dtos;
using SharedServices.Application.Features.Users.Queries;
using SharedServices.Core.Enums;
using static ApiAbstractions.Extensions.ApiResponseExtensions;

namespace SharedServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _sender;

        public UsersController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        [Authorize(Policy = nameof(PermissionType.ViewUsers))]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllUsersQuery(), cancellationToken);
            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [Authorize(Policy = nameof(PermissionType.ViewUsers))]
        public async Task<IActionResult> GetUserById(string id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetUserByIdQuery { Id = id }, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPost]
        [Authorize(Policy = nameof(PermissionType.CreateUser))]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = nameof(PermissionType.UpdateUser))]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
        {
            if (id != command.Id)
                return BadRequest(ApiResponse<UserResponseDto>.FailResponse(new List<string> { "Id in route and body must match" }));

            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = nameof(PermissionType.DeleteUser))]
        public async Task<IActionResult> DeleteUser(string id, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new DeleteUserCommand { Id = id }, cancellationToken);
            return result.ToActionResult();
        }
    }
}
