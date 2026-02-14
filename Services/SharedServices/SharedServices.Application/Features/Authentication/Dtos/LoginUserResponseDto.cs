namespace SharedServices.Application.Features.Authentication.Dtos
{
    public class LoginUserResponseDto
    {
        //public required string UserId { get; set; }
        //public required string Username { get; set; }
        //public required string Role { get; set; }
        //public long TenantId { get; set; }
        //public required List<string> Permissions { get; set; }
        public required string AccessToken { get; set; }
    }
}
