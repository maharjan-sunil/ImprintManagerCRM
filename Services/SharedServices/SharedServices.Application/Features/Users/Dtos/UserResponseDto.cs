using SharedServices.Application.Features.Users.Common.Models;

namespace SharedServices.Application.Features.Users.Dtos
{
    public class UserResponseDto: UserBase
    {
        public required string Id { get; set; }
    }
}
