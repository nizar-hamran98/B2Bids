using SharedKernel;

namespace Identity.Domain.Entities;
public class PasswordHistory : BaseEntity
{
    public long UserId { get; set; }

    public  string PasswordHash { get; set; }

    public virtual  User User { get; set; }
}
