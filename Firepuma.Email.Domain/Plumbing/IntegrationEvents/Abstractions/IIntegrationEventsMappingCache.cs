using Firepuma.BusMessaging.Abstractions.Services.Results;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

public interface IIntegrationEventsMappingCache
{
    bool IsIntegrationEventForEmailService(BusMessageEnvelope envelope);
}