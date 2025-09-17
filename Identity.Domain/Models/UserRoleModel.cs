namespace Identity.Domain.Models;
public class UserRoleModel
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public long RoleId { get; set; }
    public  RoleModel Role { get; set; }
}
