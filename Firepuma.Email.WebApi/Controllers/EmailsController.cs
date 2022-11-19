using Firepuma.BusMessaging.GooglePubSub.Models;
using Firepuma.Dtos.Email.BusMessages;
using Firepuma.Email.Domain.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Firepuma.Email.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailsController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmailsController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> SendEmailAsync(
        [FromBody] PubSubMessageEnvelope<SendEmailRequest> receivedBusMessageEnvelope,
        CancellationToken cancellationToken)
    {
        var sendEmailRequest = receivedBusMessageEnvelope.Message.Data;

        var command = new SendEmailCommand.Payload
        {
            ApplicationId = sendEmailRequest!.ApplicationId,
            TemplateId = sendEmailRequest.TemplateId,
            TemplateData = sendEmailRequest.TemplateData,
            Subject = sendEmailRequest.Subject,
            ToEmail = sendEmailRequest.ToEmail,
            ToName = sendEmailRequest.ToName,
            HtmlBody = sendEmailRequest.HtmlBody,
            TextBody = sendEmailRequest.TextBody,
        };

        await _mediator.Send(command, cancellationToken);

        return NoContent();
    }
}