using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents;

public static class ServiceCollectionExtensions
{
    public static void AddIntegrationEventsDomain(
        this IServiceCollection services)
    {
        services.AddTransient<IIntegrationEventsMappingCache, IntegrationEventsMappingCache>();
        services.AddTransient<IIntegrationEventHandler, IntegrationEventHandler>();
    }
}