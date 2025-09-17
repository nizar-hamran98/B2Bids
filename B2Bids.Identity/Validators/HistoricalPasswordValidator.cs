using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Identity.Domain.Validators;

public class HistoricalPasswordValidator : IPasswordValidator<User>
{
    public Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user, string password)
    {
        return password.Contains("testhistoricalpassword")
            ? Task.FromResult(IdentityResult.Failed(new IdentityError
            {
                Code = "HistoricalPassword",
                Description = "HistoricalPasswordValidator testing.",
            }))
            : Task.FromResult(IdentityResult.Success);
    }
}
