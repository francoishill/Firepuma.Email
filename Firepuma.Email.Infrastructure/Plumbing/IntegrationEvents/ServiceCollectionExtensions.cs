using Firepuma.BusMessaging.GooglePubSub;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Email.Infrastructure.Plumbing.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static void AddIntegrationEvents(
        this IServiceCollection services)
    {
        services.AddGooglePubSubMessageParser();

        services.AddIntegrationEventsDomain();
    }
}