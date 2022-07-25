using Azure.Messaging.ServiceBus;
using Firepuma.Email.Abstractions.Infrastructure.Validation;
using Firepuma.Email.Abstractions.Models.Dtos.ServiceBusMessages;
using Firepuma.Email.Client.Services.Results;
using Newtonsoft.Json;

namespace Firepuma.Email.Client.Services;

public class ServiceBusEmailEnqueuingClient : IEmailEnqueuingClient
{
    private readonly ServiceBusSender _serviceBusSender;

    public ServiceBusEmailEnqueuingClient(ServiceBusSender serviceBusSender)
    {
        _serviceBusSender = serviceBusSender;
    }

    public async Task<EnqueueEmailResult> EnqueueEmail(
        SendEmailRequestDto requestDto,
        CancellationToken cancellationToken)
    {
        if (!ValidationHelpers.ValidateDataAnnotations(requestDto, out var validationResults))
        {
            return EnqueueEmailResult.Failed(EnqueueEmailResult.FailureReason.InputValidationFailed, validationResults.Select(r => r.ErrorMessage).ToArray());
        }

        var messageJson = JsonConvert.SerializeObject(requestDto);
        var message = new ServiceBusMessage(messageJson)
        {
            ApplicationProperties =
            {
                ["applicationId"] = requestDto.ApplicationId,
            },
        };

        await _serviceBusSender.SendMessageAsync(message, cancellationToken);

        return EnqueueEmailResult.Success();
    }
}