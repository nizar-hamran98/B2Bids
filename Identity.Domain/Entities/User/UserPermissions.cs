using SharedKernel;

namespace Identity.Domain.Entities;
public class UserPermissions : BaseEntity
{
    public long UserId { get; set; }
    public long PermissionId { get; set; }

    public User User { get; set; }

    public Permissions Permission { get; set; }
}
