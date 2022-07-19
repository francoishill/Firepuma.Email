using System.ComponentModel.DataAnnotations;

namespace ExampleSendEmailApi.Config;

public class ServiceBusOptions
{
    [Required]
    public string ConnectionString { get; set; }

    [Required]
    public string QueueName { get; set; }
}