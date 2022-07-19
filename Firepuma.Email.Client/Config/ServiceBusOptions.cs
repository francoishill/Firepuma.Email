using System.ComponentModel.DataAnnotations;

namespace Firepuma.Email.Client.Config;

internal class ServiceBusOptions
{
    [Required]
    public string ServiceBus { get; set; }

    [Required]
    public string QueueName { get; set; }
}