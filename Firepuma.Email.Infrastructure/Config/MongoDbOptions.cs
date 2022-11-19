using System.ComponentModel.DataAnnotations;

namespace Firepuma.Email.Infrastructure.Config;

public class MongoDbOptions
{
    [Required]
    public string ConnectionString { get; set; } = null!;

    [Required]
    public string DatabaseName { get; set; } = null!;

    [Required]
    public string AuthorizationFailuresCollectionName { get; set; } = null!;

    [Required]
    public string CommandExecutionsCollectionName { get; set; } = null!;

    [Required]
    public string PetsCollectionName { get; set; } = null!;
}