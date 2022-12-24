﻿namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.ValueObjects;

public class IntegrationEventEnvelope
{
    public required string EventId { get; init; }
    public required string EventType { get; init; }
    public required string EventPayload { get; init; }
}