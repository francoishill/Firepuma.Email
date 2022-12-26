using Firepuma.BusMessaging.GooglePubSub;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Services;
using Firepuma.EventMediation.IntegrationEvents;
using Firepuma.EventMediation.IntegrationEvents.CommandExecution.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Email.Infrastructure.Plumbing.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static void AddIntegrationEvents(
        this IServiceCollection services)
    {
        services.AddGooglePubSubMessageParser();

        services.AddTransient<IIntegrationEventsMappingCache, IntegrationEventsMappingCache>();

        services.AddIntegrationEventReceiving<
            IntegrationEventsMappingCache,
            IntegrationEventWithCommandsFactoryHandler>();
    }
}