using SharedKernel;

namespace Identity.Domain.Models;
public class PermissionModel
{
    public long Id { get; set; }
    public  string Code { get; set; }
    public  string Name { get; set; }
    public EntityStatus Status { get; set; }
    public string? StatusName { get; set; }
    public string? PageName { get; set; }
    public int ParentId { get; set; }
    public bool Checked { get; set; } = false;
}
