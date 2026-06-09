using System.ComponentModel.DataAnnotations;

namespace CookWizard.Infrastructure.Common.Settings;

public class MongoCollectionSettings
{
    public string Name { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string Collection { get; set; } = string.Empty;
}

public class MongoSettings
{
    [Required(ErrorMessage = "The server is required.")]
    public string Server { get; set; } = string.Empty;
    [Required(ErrorMessage = "The port is required.")]
    public int Port { get; set; }
    [Required(ErrorMessage = "The user is not set.")]
    public string User { get; set; } = string.Empty;
    [Required(ErrorMessage = "The password is not set.")]
    public string Password { get; set; } = string.Empty;
    [Required(ErrorMessage = "AppName is not set.")]
    public string AppName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Collections are required.")]
    public required MongoCollectionSettings[] Collections { get; set; }
}
