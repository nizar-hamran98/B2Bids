using System.ComponentModel.DataAnnotations;
namespace SharedKernel;

public class DBConnectionStrings
{
    [Required]
    public const string ConfigurationSection = "ConnectionStrings";

    [Required]
    public string DB { get; set; }
}
public class AzureAuthentication
{
    [Required]
    public const string ConfigurationSection = "AzureAd";

    [Required, Url]
    public string Instance { get; set; }

    [Required]
    public string TenantId { get; set; }

    [Required]
    public string ClientId { get; set; }

    [Required]
    public string ClientSecret { get; set; }
}
public class DMSOptions
{
    [Required]
    public const string ConfigurationSection = "DMSOptions";

    [Required, Url]
    public string PostFileBaseUri { get; set; }
    [Required, Url]
    public string PostFolderBaseUri { get; set; }
    [Required, Url]
    public string GetFolderByIDBaseUri { get; set; }
    [Required, Url]
    public string DeleteFolderByIDBaseUri { get; set; }
    [Required, Url]
    public string DowloadFileByIdBaseUri { get; set; }
    [Required, Url]
    public string DowloadFolderBaseUri { get; set; }
    [Required, Url]
    public string PostFilesBaseUri { get; set; }
    [Required, Url]
    public string DeleteFileByIDBaseUri { get; set; }
    [Required, Url]
    public string DeleteFilesByIDsBaseUri { get; set; }

}