using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Firepuma.Email.Abstractions.Infrastructure.Validation;
using Firepuma.Email.Abstractions.Models.Dtos.ServiceBusMessages;
using Firepuma.Email.FunctionApp.Features.Emails.Commands;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Firepuma.Email.FunctionApp.Api;

public class SendEmailTrigger
{
    private readonly IMediator _mediator;

    public SendEmailTrigger(IMediator mediator)
    {
        _mediator = mediator;
    }

    [FunctionName("SendEmailTrigger")]
    public async Task RunAsync(
        [ServiceBusTrigger("%QueueName%", Connection = "ServiceBus")] string emailMessageRequest,
        string messageId,
        ILogger log,
        CancellationToken cancellationToken)
    {
        log.LogInformation("C# ServiceBus queue trigger function processing message ID {Id}", messageId);

        var requestDto = JsonConvert.DeserializeObject<SendEmailRequestDto>(emailMessageRequest);

        log.LogInformation(
            "Processing request for ToEmail '{ToEmail}', Subject '{Subject}' and ApplicationId '{ApplicationId}'",
            requestDto.ToEmail, requestDto.Subject, requestDto.ApplicationId);

        if (!ValidationHelpers.ValidateDataAnnotations(requestDto, out var validationResultsForRequest))
        {
            throw new Exception(string.Join(" ", new[] { "Request body is invalid." }.Concat(validationResultsForRequest.Select(s => s.ErrorMessage))));
        }

        var command = new SendEmail.Command
        {
            TemplateId = requestDto.TemplateId,
            TemplateData = requestDto.TemplateData,

            Subject = requestDto.Subject,

            ToEmail = requestDto.ToEmail,
            ToName = requestDto.ToName,

            HtmlBody = requestDto.HtmlBody,
            TextBody = requestDto.TextBody,
        };

        await _mediator.Send(command, cancellationToken);
    }
}