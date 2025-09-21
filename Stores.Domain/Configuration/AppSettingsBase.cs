using System.ComponentModel.DataAnnotations;

namespace Stores.Domain;
public class AppSettingsBase
{
    public class Authentication
    {
        [Required]
        public const string ConfigurationSection = "Authentication";

        [Required]
        public short NumberOfLoginRetry { get; set; }
    }
}
