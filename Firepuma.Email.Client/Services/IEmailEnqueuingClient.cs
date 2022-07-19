using Firepuma.Email.Abstractions.Models.Dtos.ServiceBusMessages;
using Firepuma.Email.Abstractions.Models.ValueObjects;
using Firepuma.Email.Client.Models.ValueObjects;

namespace Firepuma.Email.Client.Services;

public interface IEmailEnqueuingClient
{
    Task<SuccessOrFailure<SuccessfulResult, FailedResult>> EnqueueEmail(SendEmailRequestDto requestDto, CancellationToken cancellationToken);
}