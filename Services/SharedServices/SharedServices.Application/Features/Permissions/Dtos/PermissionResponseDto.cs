using SharedServices.Application.Features.Permissions.Common.Models;

namespace SharedServices.Application.Features.Permissions.Dtos
{
    public class PermissionResponseDto: PermissionBase
    {
        public long PermissionId { get; set; }
        public bool IsActive { get; set; }
    }
}
