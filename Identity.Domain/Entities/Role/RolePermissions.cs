using SharedKernel;

namespace Identity.Domain.Entities;
public class RolePermissions : BaseEntity
{
    public long RoleId { get; set; }
    public long PermissionId { get; set; }
    public Role Role { get; set; }
    public Permissions? Permission { get; set; }

    public static RolePermissions[] GenerateRolePermissionsSeedData() =>
         Enumerable.Range(1, 18).Select(i => new RolePermissions
         {
             Id = i,
             RoleId = 1,
             PermissionId = i,
             CreatedAt = DateTime.Now,
             UpdatedAt = null,
             StatusId = 1,
             CreatedBy = "",
             UpdatedBy = ""
         }).ToArray();
}