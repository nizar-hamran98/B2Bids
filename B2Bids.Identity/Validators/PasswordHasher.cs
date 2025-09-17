using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Validators;

public interface IPasswordHasher
{
    bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}
public class PasswordHasher : IPasswordHasher
{
    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
    }
}
