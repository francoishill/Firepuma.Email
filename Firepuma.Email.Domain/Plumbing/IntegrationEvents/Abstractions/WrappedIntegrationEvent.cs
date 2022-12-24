using Firepuma.CommandsAndQueries.Abstractions.Commands;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.ValueObjects;
using MediatR;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

public class WrappedIntegrationEvent<TEvent> : IRequest<IEnumerable<ICommandRequest>>
{
    public string EventSourceId { get; init; }
    public IntegrationEventEnvelope EventEnvelope { get; init; }
    public TEvent EventPayload { get; init; }

    public WrappedIntegrationEvent(string eventSourceId, IntegrationEventEnvelope eventEnvelope, TEvent eventPayload)
    {
        EventSourceId = eventSourceId;
        EventEnvelope = eventEnvelope;
        EventPayload = eventPayload;
    }
}