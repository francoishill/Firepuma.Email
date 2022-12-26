using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Firepuma.BusMessaging.Abstractions.Services.Results;
using Firepuma.Dtos.Email.BusMessages;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;
using Firepuma.EventMediation.IntegrationEvents.Abstractions;
using Firepuma.EventMediation.IntegrationEvents.ValueObjects;

[assembly: InternalsVisibleTo("Firepuma.Email.Tests")]

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Services;

public class IntegrationEventsMappingCache :
    IIntegrationEventsMappingCache,
    IIntegrationEventDeserializer
{
    private static bool IsIntegrationEventForEmailService(string messageType)
    {
        return messageType.StartsWith("Firepuma/EmailService/", StringComparison.OrdinalIgnoreCase);
    }

    public bool IsIntegrationEventForEmailService(BusMessageEnvelope envelope)
    {
        return IsIntegrationEventForEmailService(envelope.MessageType);
    }

    public bool TryDeserializeIntegrationEvent(
        IntegrationEventEnvelope envelope,
        [NotNullWhen(true)] out object? eventPayload)
    {
        eventPayload = envelope.EventType switch
        {
            "Firepuma/EmailService/SendEmail" => DeserializePayload<SendEmailRequest>(envelope.EventPayload),

            _ => null,
        };

        return eventPayload != null;
    }

    private static TIntegrationEvent? DeserializePayload<TIntegrationEvent>(string eventPayload)
    {
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        return JsonSerializer.Deserialize<TIntegrationEvent?>(eventPayload, options);
    }
}