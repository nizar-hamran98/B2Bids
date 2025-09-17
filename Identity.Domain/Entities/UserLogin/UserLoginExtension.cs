
namespace Identity.Domain.Entities;
public sealed class UserLoginExtension : UserLogin
{
    protected UserLoginExtension(string token, long userId, string issuedbyIp, string lastUsedIp, string userAgent,
        bool isRefreshable, int? expiryInMinutes)
    {
        UserId = userId;
        IssuedByIp = issuedbyIp;
        LastUsedIp = lastUsedIp;
        UserAgent = userAgent;
        Token = token;
        UserAgent = userAgent;
        IssuedAt = DateTime.UtcNow;
        LastLoginIn = DateTime.UtcNow;
        RefreshTokenId = isRefreshable ? Guid.NewGuid().ToString() : null;
        IsRefreshable = isRefreshable;
        ExpiryDate = expiryInMinutes.HasValue ? DateTime.UtcNow.AddMinutes(expiryInMinutes.Value) : null;
    }

    private static UserLogin CreateSession(string token, long userId, string issuedbyIp, string lastUsedIp,
        string userAgent, bool isRefreshable, int? expiryInMinutes)
    {
        return new UserLoginExtension(token, userId, issuedbyIp, lastUsedIp, userAgent, isRefreshable, expiryInMinutes);
    }

    public static UserLogin CreateAPISession(string token, long userId, string issuedbyIp, string lastUsedIp
        , string userAgent)
    {
        return CreateSession(token, userId, issuedbyIp, lastUsedIp, userAgent, false, null);
    }

    public static UserLogin CreateUISession(string token, long userId, string issuedbyIp, string lastUsedIp,
        string userAgent, int expiryInMinutes)
    {
        return CreateSession(token, userId, issuedbyIp, lastUsedIp, userAgent, true, expiryInMinutes);
    }

    public static void ReLoginSession(UserLogin userLogin, string token, string lastUsedIp, int expiryInMinutes)
    {
        userLogin.LastUsedIp = lastUsedIp;
        userLogin.LastLoginIn = DateTime.UtcNow;
        userLogin.Token = token;
        userLogin.ExpiryDate = DateTime.UtcNow.AddMinutes(expiryInMinutes);
        userLogin.RefreshTokenId = userLogin.IsRefreshable ? Guid.NewGuid().ToString() : null;
    }

    public static void RefreshSession(UserLogin userLogin, string token, string lastUsedIp)
    {
        userLogin.Token = token;
        userLogin.RefreshTokenId = Guid.NewGuid().ToString();
        userLogin.LastUsedIp = lastUsedIp;
        userLogin.LastRefreshToken = DateTime.UtcNow;
    }

    public static void LogoutSession(UserLogin userLogin, string lastUsedIp)
    {
        userLogin.LastUsedIp = lastUsedIp;
        userLogin.RefreshTokenId = null;
        userLogin.ExpiryDate = null;
        userLogin.LastSignOut = DateTime.UtcNow;
    }
}
