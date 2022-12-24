using System.Diagnostics.CodeAnalysis;
using Firepuma.BusMessaging.Abstractions.Services.Results;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.ValueObjects;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

public interface IIntegrationEventsMappingCache
{
    bool IsIntegrationEventForEmailService(BusMessageEnvelope envelope);

    bool TryDeserializeIntegrationEvent(
        IntegrationEventEnvelope envelope,
        [NotNullWhen(true)] out object? eventPayload);
}