using System.Text.Json.Serialization;
using SharedKernel;

namespace Identity.Domain.Models;
public class UserModel
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public int AccessFailedCount { get; set; }
    public string? Password { get; set; }
    public EntityStatus Status { get; set; }
    public long RoleId { get; set; }
    
    public string? RoleName { get; set; }
    public bool? IsNeverExpire { get; set; }
    [JsonIgnore]
    public RoleModel? Role { get; set; }
    [JsonIgnore]
    public List<UserPermissionModel>? UserPermissions { get; set; }
    public string? UserTypeCode { get; set; }
    public bool IsAllowAccess { get; set; } = false;
}