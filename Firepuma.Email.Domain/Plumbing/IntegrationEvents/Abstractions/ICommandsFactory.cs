using Firepuma.CommandsAndQueries.Abstractions.Commands;
using MediatR;

namespace Firepuma.Email.Domain.Plumbing.IntegrationEvents.Abstractions;

public interface ICommandsFactory<TEvent> : IRequestHandler<WrappedIntegrationEvent<TEvent>, IEnumerable<ICommandRequest>>
{
}