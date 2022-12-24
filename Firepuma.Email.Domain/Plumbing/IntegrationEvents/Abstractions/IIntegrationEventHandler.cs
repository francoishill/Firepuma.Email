using Firepuma.Email.Domain.Plumbing.IntegrationEvents.ValueObjects;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

public interface IIntegrationEventHandler
{
    Task<bool> TryHandleEvent(
        string eventSourceId,
        IntegrationEventEnvelope integrationEventEnvelope,
        CancellationToken cancellationToken);
}