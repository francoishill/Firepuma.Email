using System.Text.Json;
using Firepuma.CommandsAndQueries.Abstractions.Commands;
using Firepuma.Dtos.Email.BusMessages;
using Firepuma.Email.Domain.Commands;
using Firepuma.EventMediation.IntegrationEvents.CommandExecution.Abstractions;
using Microsoft.Extensions.Logging;

// ReSharper disable InlineTemporaryVariable
// ReSharper disable UnusedType.Global

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.CommandFactories;

public class SendEmailRequestCommandsFactory : ICommandsFactory<SendEmailRequest>
{
    private readonly ILogger<SendEmailRequestCommandsFactory> _logger;

    public SendEmailRequestCommandsFactory(
        ILogger<SendEmailRequestCommandsFactory> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<ICommandRequest>> Handle(
        CreateCommandsFromIntegrationEventRequest<SendEmailRequest> request,
        CancellationToken cancellationToken)
    {
        var eventPayload = request.EventPayload;

        _logger.LogDebug(
            "Creating SendEmailCommand from integration event payload: {Payload}",
            JsonSerializer.Serialize(eventPayload));

        var command = new SendEmailCommand.Payload
        {
            ApplicationId = request.EventSourceId,
            TemplateId = eventPayload.TemplateId,
            TemplateData = eventPayload.TemplateData,
            Subject = eventPayload.Subject,
            FromEmail = eventPayload.FromEmail,
            ToEmail = eventPayload.ToEmail,
            ToName = eventPayload.ToName,
            HtmlBody = eventPayload.HtmlBody,
            TextBody = eventPayload.TextBody,
            GroupId = eventPayload.GroupId,
            GroupsToDisplay = eventPayload.GroupsToDisplay,
        };

        await Task.CompletedTask;
        return new[] { command };
    }
}