using System.Text.Json;
using Firepuma.BusMessaging.Abstractions.Services;
using Firepuma.Dtos.Email.BusMessages;
using Firepuma.Email.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Firepuma.Email.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PubSubListenerController : ControllerBase
{
    private readonly IMessageBusDeserializer _messageBusDeserializer;
    private readonly IMediator _mediator;

    public PubSubListenerController(
        IMessageBusDeserializer messageBusDeserializer,
        IMediator mediator)
    {
        _messageBusDeserializer = messageBusDeserializer;
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> HandleBusMessageAsync(
        JsonElement envelope,
        CancellationToken cancellationToken)
    {
        if (!_messageBusDeserializer.TryDeserializeMessage(envelope, out var deserializedMessage, out var messageExtraDetails, out var validationError))
        {
            return BadRequest(validationError);
        }

        if (deserializedMessage is SendEmailRequest sendEmailRequest)
        {
            await SendEmail(sendEmailRequest, messageExtraDetails.SourceId, cancellationToken);
        }
        else
        {
            return BadRequest($"Unsupported message type '{deserializedMessage.GetType().FullName}'");
        }

        return NoContent();
    }

    private async Task SendEmail(SendEmailRequest sendEmailRequest, string sourceId, CancellationToken cancellationToken)
    {
        var command = new SendEmailCommand.Payload
        {
            ApplicationId = sourceId,
            TemplateId = sendEmailRequest.TemplateId,
            TemplateData = sendEmailRequest.TemplateData,
            Subject = sendEmailRequest.Subject,
            FromEmail = sendEmailRequest.FromEmail,
            ToEmail = sendEmailRequest.ToEmail,
            ToName = sendEmailRequest.ToName,
            HtmlBody = sendEmailRequest.HtmlBody,
            TextBody = sendEmailRequest.TextBody,
            GroupId = sendEmailRequest.GroupId,
            GroupsToDisplay = sendEmailRequest.GroupsToDisplay,
        };

        await _mediator.Send(command, cancellationToken);
    }
}