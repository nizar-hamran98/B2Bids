using SharedKernel;

namespace Identity.Domain.Entities;
public class Role : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; } 
    public  IList<RolePermissions> RolePermissions { get; set; }
    public  IList<UserRole> UserRoles { get; set; }
    public int? ParentId { get; set; }
    public bool IsDefault { get; set; } = false;

    // Seeding
    public static Role[] GenerateRoleSeedData()
    {
        return new Role[]
        {
             new() {
             Id =1,
             Name ="Administrator",
             Description = "Administrator",
             IsDefault = true, 
             StatusId = 1,
             CreatedAt = new DateTime(2024,09,12,12,0,0)
             }
        };
    }
}