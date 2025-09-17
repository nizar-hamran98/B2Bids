using SharedKernel;

namespace Identity.Domain.Models;
public class UserPermissionModel
{
    public long Id { get; set; }
    public EntityStatus Status { get; set; }
    public long UserId { get; set; }
    public long PermissionId { get; set; }
    public required string PermissionCode { get; set; }
}
