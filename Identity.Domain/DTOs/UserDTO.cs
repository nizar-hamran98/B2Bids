namespace Identity.Domain.DTOs;
public class UserDTO
{
    public long Id { get; set; }
    public string UserName { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool LockoutEnabled { get; set; }
    public DateTimeOffset? LockoutEnd { get; set; }
    public int AccessFailedCount { get; set; }
    public string? UserTypeCode { get; set; }
    public object token { get; set; }
    public string? ExternalCode { get; set; } 
}
