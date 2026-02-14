using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Application.Features.Permissions.Queries;
using static ApiAbstractions.Extensions.ApiResponseExtensions;

namespace SharedServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        private readonly ISender _sender;

        public PermissionsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissions(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetAllPermissionsQuery(), cancellationToken);
            return result.ToActionResult();
        }
    }
}
