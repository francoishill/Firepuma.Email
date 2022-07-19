using Azure.Messaging.ServiceBus;
using Firepuma.Email.Client.Config;
using Firepuma.Email.Client.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

// ReSharper disable RedundantTypeArgumentsOfMethod

namespace Firepuma.Email.Client;

public static class ServiceCollectionExtensions
{
    public static void AddEmailServiceClient(
        this IServiceCollection services,
        IConfiguration emailServiceClientConfigurationSection)
    {
        services.ConfigureAndValidate<ServiceBusOptions>(emailServiceClientConfigurationSection);

        services.AddSingleton<ServiceBusSender>(s =>
        {
            var options = s.GetRequiredService<IOptions<ServiceBusOptions>>();

            var serviceBusClient = new ServiceBusClient(options.Value.ServiceBus);

            return serviceBusClient.CreateSender(options.Value.QueueName);
        });

        services.AddSingleton<IEmailEnqueuingClient, ServiceBusEmailEnqueuingClient>();
    }
}