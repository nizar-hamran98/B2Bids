using SharedKernel;

namespace Identity.Domain.Models;
public class RoleModel
{
    public long Id { get; set; }

    public virtual  string Name { get; set; }

    public virtual  string Description { get; set; } 
    public EntityStatus Status { get; set; }

    public int? ParentId { get; set; }
    public bool IsDefault { get; set; }
    public List<string>? RolePermissions { get; set; }
}
