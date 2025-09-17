using SharedKernel;

namespace Identity.Domain.Models;
public class RolePermissionModel
{
    public long Id { get; set; }
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public EntityStatus Status { get; set; }

    public string? PermissionName { get; set; }

    public string? PermissionCode { get; set; }
}
