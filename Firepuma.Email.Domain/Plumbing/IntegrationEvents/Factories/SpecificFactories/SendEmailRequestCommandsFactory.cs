using Firepuma.CommandsAndQueries.Abstractions.Commands;
using Firepuma.Dtos.Email.BusMessages;
using Firepuma.Email.Domain.Commands;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

// ReSharper disable InlineTemporaryVariable
// ReSharper disable UnusedType.Global

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Factories.SpecificFactories;

public class SendEmailRequestCommandsFactory : ICommandsFactory<SendEmailRequest>
{
    public async Task<IEnumerable<ICommandRequest>> Handle(
        WrappedIntegrationEvent<SendEmailRequest> request,
        CancellationToken cancellationToken)
    {
        var eventPayload = request.EventPayload;

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