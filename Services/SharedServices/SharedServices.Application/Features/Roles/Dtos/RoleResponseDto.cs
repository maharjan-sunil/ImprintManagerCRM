using SharedServices.Application.Features.Roles.Common.Models;

namespace SharedServices.Application.Features.Roles.Dtos
{
    public class RoleResponseDto: RoleBase
    {
        public required string Id { get; set; }
    }
}
