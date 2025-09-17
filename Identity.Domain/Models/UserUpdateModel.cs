using SharedKernel;
using System.Text.Json.Serialization;

namespace Identity.Domain.Models;
public class UserUpdateModel
{
    public string? UserName { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public EntityStatus? Status { get; set; }
    public long RoleId { get; set; }
    [JsonIgnore]
    public RoleModel? Role { get; set; }
    [JsonIgnore]
    public List<UserPermissionModel>? UserPermissions { get; set; }
    public string? UserTypeCode { get; set; }
    public bool? IsAllowAccess { get; set; }
    public bool? IsNeverExpire { get; set; }
}
