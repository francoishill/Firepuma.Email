using System.Diagnostics;
using Firepuma.CommandsAndQueries.Abstractions.Commands;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;
using Firepuma.Email.Domain.Plumbing.IntegrationEvents.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Services;

internal class IntegrationEventHandler : IIntegrationEventHandler
{
    private readonly ILogger<IntegrationEventHandler> _logger;
    private readonly IIntegrationEventsMappingCache _mappingCache;
    private readonly IMediator _mediator;

    public IntegrationEventHandler(
        ILogger<IntegrationEventHandler> logger,
        IIntegrationEventsMappingCache mappingCache,
        IMediator mediator)
    {
        _logger = logger;
        _mappingCache = mappingCache;
        _mediator = mediator;
    }

    public async Task<bool> TryHandleEvent(
        string eventSourceId,
        IntegrationEventEnvelope integrationEventEnvelope,
        CancellationToken cancellationToken)
    {
        if (!_mappingCache.TryDeserializeIntegrationEvent(integrationEventEnvelope, out var eventPayload))
        {
            _logger.LogError(
                "Unable to deserialize integration event with type {Type} and id {Id}",
                integrationEventEnvelope.EventType, integrationEventEnvelope.EventId);
            return false;
        }

        var createCommandsRequest = CreateCommandsRequest(eventSourceId, integrationEventEnvelope, eventPayload);

        var commandCreationStopwatch = Stopwatch.StartNew();
        // this mediator Send will be handled by the appropriate ICommandsFactory<> implementation / handler
        var commands = (await _mediator.Send(createCommandsRequest, cancellationToken)).ToArray();
        commandCreationStopwatch.Stop();

        if (commands.Length == 0)
        {
            _logger.LogInformation(
                "No commands are mapped to integration event type {EventType} id {IntegrationEventId}, creation attempt took {DurationMs} ms",
                eventPayload.GetType().FullName, integrationEventEnvelope.EventId, commandCreationStopwatch.ElapsedMilliseconds);
            return true;
        }

        _logger.LogDebug(
            "Creation (not yet execution) of {Count} commands took {DurationMs} ms",
            commands.Length, commandCreationStopwatch.ElapsedMilliseconds);

        var successCount = 0;
        var errorCount = 0;

        var executionStopwatch = Stopwatch.StartNew();
        await Task.WhenAll(commands.Select(
            async command =>
            {
                try
                {
                    await _mediator.Send(command, cancellationToken);
                    Interlocked.Increment(ref successCount);
                }
                catch (Exception exception)
                {
                    _logger.LogError(
                        exception,
                        "Failed to execute command {Type} with id {CommandId}, from integration event type {EventType} and id {IntegrationEventId}",
                        command.GetType().FullName, command.CommandId, eventPayload.GetType().FullName, integrationEventEnvelope.EventId);

                    Interlocked.Increment(ref errorCount);
                }
            }));
        executionStopwatch.Stop();

        if (successCount > 0)
        {
            _logger.LogInformation(
                "Successfully executed {Count}/{Total} commands in {Milliseconds} ms caused by integration event type {EventType} and id {IntegrationEventId}",
                successCount, commands.Length, executionStopwatch.ElapsedMilliseconds, eventPayload.GetType().FullName, integrationEventEnvelope.EventId);
        }

        return errorCount == 0;
    }

    private static IRequest<IEnumerable<ICommandRequest>> CreateCommandsRequest(
        string eventSourceId,
        IntegrationEventEnvelope eventEnvelope,
        object eventPayload)
    {
        var type = eventPayload.GetType();
        var createCommandsRequest = (IRequest<IEnumerable<ICommandRequest>>)
            (Activator.CreateInstance(
                 typeof(WrappedIntegrationEvent<>).MakeGenericType(type),
                 args: new[] { eventSourceId, eventEnvelope, eventPayload })
             ?? throw new InvalidOperationException($"Could not create wrapper type for {type}"));
        return createCommandsRequest;
    }
}