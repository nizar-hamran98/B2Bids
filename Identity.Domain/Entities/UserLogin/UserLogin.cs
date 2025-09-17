using SharedKernel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Identity.Domain.Entities;
public class UserLogin : BaseEntity
{
    public  string Token { get; set; }
    public DateTime IssuedAt { get; init; }
    public DateTime? ExpiryDate { get; set; }
    public string? IssuedByIp { get; init; }
    public string? LastUsedIp { get; set; }
    public DateTime? LastLoginIn { get; set; }
    public DateTime? LastSignOut { get; set; }
    public DateTime? LastRefreshToken { get; set; }
    public string? RefreshTokenId { get; set; }
    public bool IsRefreshable { get; init; } = false;
    public string? UserAgent { get; set; }
    [ForeignKey("User")]
    public long UserId { get; init; }
    public User User { get; }
}