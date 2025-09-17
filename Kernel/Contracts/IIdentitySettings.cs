using Kernel.IdentitySettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Contract;
public interface IIdentitySettings
{
    AzureSamlIdentitySettings AzureSamlIdentitySettings { get; set; }
    JwtSettings JwtSettings { get; set; }
    UserLoginSetting UserLoginSetting { get; set; }
}
public interface IAzureSamlIdentitySettings
{
    int TokenExpiryInSeconds { get; set; }
    string IssuerBaseUrl { get; set; }
    string LoginBaseUrl { get; set; }
    int SessionVolatilityInSeconds { get; set; }
    string AllowedHosts { get; set; }
}
public interface IJwtSettings
{
    public string SecretKey { get; set; }
    public int TokenExpiryInSeconds { get; set; }
}
public interface IUserLoginSetting
{
    public int TokenRefreshExpiryInMinutes { get; set; }
    public int TokenExpiryInSeconds { get; set; }
    public int DelayBeforeLoggingOutInMinutes { get; set; }
}
