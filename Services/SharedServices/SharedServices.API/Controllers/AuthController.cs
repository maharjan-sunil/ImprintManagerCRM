using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedServices.Application.Features.Authentication.Commands;
using static ApiAbstractions.Extensions.ApiResponseExtensions;

namespace SharedServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result.ToActionResult();
        }
    }
}
