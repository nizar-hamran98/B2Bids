using SharedKernel;

namespace Identity.Domain.Entities;
public class UserRole : BaseEntity
{
    public long UserId { get; set; }
    public  User User { get; set; }
    public long RoleId { get; set; }
    public Role Role { get; set; }
}
