using Kernel.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using SharedKernel;

namespace Kernel.IdentitySettings;
public sealed class IdentitySettings : IIdentitySettings
{
    public AzureSamlIdentitySettings AzureSamlIdentitySettings { get; set; }
    public JwtSettings JwtSettings { get; set; }
    public UserLoginSetting UserLoginSetting { get; set; }

    private IdentitySettings()
    {

    }
    public IdentitySettings(IHostEnvironment env)
    {
        var configurationBuilder = new ConfigurationBuilder();
        var instance = new IdentitySettings();
        configurationBuilder.AddJsonFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "identitySettings.json"), false, true);
        configurationBuilder.Build().Bind(instance);
        JwtSettings = instance.JwtSettings;
        AzureSamlIdentitySettings = instance.AzureSamlIdentitySettings;
        UserLoginSetting = instance.UserLoginSetting;
    }
}
public sealed class AzureSamlIdentitySettings : IAzureSamlIdentitySettings
{
    string issuerBaseUrl { get; set; }
    string loginBaseUrl { get; set; }
    public int TokenExpiryInSeconds { get; set; }
    public string IssuerBaseUrl { get { return issuerBaseUrl; } set { issuerBaseUrl = value.NormalizeUrl(); } }
    public string LoginBaseUrl { get { return loginBaseUrl; } set { loginBaseUrl = value.NormalizeUrl(); } }
    public string AllowedHosts { get; set; }
    public int SessionVolatilityInSeconds { get { return sessionVolatilityInSeconds == 0 ? 15 : sessionVolatilityInSeconds; } set { sessionVolatilityInSeconds = value; } }
    int sessionVolatilityInSeconds = 0;
}
public sealed class JwtSettings : IJwtSettings
{
    string secretKey = "";
    public string SecretKey { get { return string.IsNullOrWhiteSpace(secretKey) ? "jwtmiddlewaresecret" : secretKey; } set { secretKey = value; } }
    int tokenExpiryInSeconds = 0;
    public int TokenExpiryInSeconds { get { return tokenExpiryInSeconds == 0 ? 15 : tokenExpiryInSeconds; } set { tokenExpiryInSeconds = value; } }
}

public sealed class UserLoginSetting : IUserLoginSetting
{
    public int TokenRefreshExpiryInMinutes { get; set; }
    public int TokenExpiryInSeconds { get; set; }
    public int DelayBeforeLoggingOutInMinutes { get; set; }
}