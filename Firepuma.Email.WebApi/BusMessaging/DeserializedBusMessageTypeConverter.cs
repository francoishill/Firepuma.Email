using System.Text.Json;
using Firepuma.BusMessaging.Abstractions.Services.Results;
using Firepuma.BusMessaging.GooglePubSub.Abstractions;
using Firepuma.Dtos.Email.BusMessages;

namespace Firepuma.Email.WebApi.BusMessaging;

public class DeserializedBusMessageTypeConverter : IDeserializedMessageTypeConverter
{
    private readonly ILogger<DeserializedBusMessageTypeConverter> _logger;

    private readonly Dictionary<string, Type> _messageTypes;

    public DeserializedBusMessageTypeConverter(
        ILogger<DeserializedBusMessageTypeConverter> logger)
    {
        _logger = logger;

        _messageTypes = new Dictionary<string, Type>
        {
            ["Firepuma/EmailService/SendEmail"] = typeof(SendEmailRequest),
        };
    }

    public object? ConvertToApplicationType(JsonElement? deserializedMessage, MessageExtraDetails messageExtraDetails)
    {
        if (!_messageTypes.TryGetValue(messageExtraDetails.MessageType, out var messageType))
        {
            _logger.LogError("Unsupported MessageType {MessageType}, there is no mapping for it", messageExtraDetails.MessageType);
            throw new Exception($"Unsupported MessageType {messageExtraDetails.MessageType}, there is no mapping for it");
        }

        return deserializedMessage?.Deserialize(messageType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}