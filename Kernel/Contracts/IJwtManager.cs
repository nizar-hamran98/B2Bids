using System.Security.Claims;

namespace Kernel.Contract;
public interface IJwtManager
{
    public object GenerateEntityToken<T>(T session, int tokenExpiryInSeconds = 0) where T : class;
    public string GenerateToken(string userId, int tokenExpiryInSeconds = 0);
    public IEnumerable<Claim> ParseToken(string token, bool validateLifeTime = true);
    public IEnumerable<Claim> ParseTokenV2(string token);
}
