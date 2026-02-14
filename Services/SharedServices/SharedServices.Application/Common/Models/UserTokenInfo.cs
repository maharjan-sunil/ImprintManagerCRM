namespace SharedServices.Application.Common.Models
{
    public class UserTokenInfo
    {
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public required List<string> Roles { get; set; }
        public required long TenantId { get; set; }
        public required List<string> Permissions { get; set; }
    }
}
