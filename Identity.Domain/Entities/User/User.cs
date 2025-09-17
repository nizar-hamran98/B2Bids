using SharedKernel;

namespace Identity.Domain.Entities;
public class User : BaseEntity
{
    public  string UserName { get; set; }

    public  string NormalizedUserName { get; set; }
    public  string FullName { get; set; }

    public  string Email { get; set; }

    public  string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public  string PasswordHash { get; set; }

    public  string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; } 

    public bool LockoutEnabled { get; set; }

    public bool? IsNeverExpire { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public int AccessFailedCount { get; set; }

    public  IList<UserToken> Tokens { get; set; }

    public  IList<UserRole> UserRoles { get; set; }
    public  IList<UserPermissions> UserPermissions { get; set; }
    public  IList<PasswordHistory> PasswordHistories { get; set; }
    public long RoleId { get; set; }
    public Role? Role { get; set; }
    public string? UserTypeCode { get; set; }
    public bool IsAllowAccess { get; set; } = false;
    public string? ExternalCode { get; set; }

    // Seeding
    public static User[] GenerateUserSeedData()
    {
        return new User[]
        {
             new() {
             Id =1,
             FullName = "Admin",
             UserName = "admin",
             NormalizedUserName= "admin",
             PasswordHash ="KpdRbDVLaISM29j1SiJqClWyHtE44getbFy7nACqWuo=", // demo
             PhoneNumber = "123456",
             PhoneNumberConfirmed= true,
             TwoFactorEnabled = false,
             AccessFailedCount=0,
             LockoutEnabled=false,
             LockoutEnd=new DateTimeOffset(2025,11,30,12,0,0,new TimeSpan()),
             Email = "admin@gnteq.com",
             NormalizedEmail = "ADMIN@GNTEQ.COM",
             EmailConfirmed= true,
             RoleId=1,
             StatusId = 1,
             CreatedAt = new DateTimeOffset(2022,11,30,12,0,0,new TimeSpan()).DateTime,
             IsAllowAccess= true
             },
        };
    }
}
