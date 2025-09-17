using SharedKernel;

namespace Identity.Domain.DTOs;
public class UserPermissionDTO
{
    public long Id { get; set; }
    public EntityStatus Status { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public string PermissionCode { get; set; }
}
