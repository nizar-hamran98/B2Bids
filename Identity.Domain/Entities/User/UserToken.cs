using SharedKernel;

namespace Identity.Domain.Entities;
public class UserToken : BaseEntity
{
    public long UserId { get; set; }

    public  string LoginProvider { get; set; }

    public  string TokenName { get; set; }

    public  string TokenValue { get; set; }
    public  User User { get; set; }
}
