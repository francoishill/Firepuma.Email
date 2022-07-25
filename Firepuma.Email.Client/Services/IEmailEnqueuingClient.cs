using Firepuma.Email.Abstractions.Models.Dtos.ServiceBusMessages;
using Firepuma.Email.Client.Services.Results;

namespace Firepuma.Email.Client.Services;

public interface IEmailEnqueuingClient
{
    Task<EnqueueEmailResult> EnqueueEmail(SendEmailRequestDto requestDto, CancellationToken cancellationToken);
}