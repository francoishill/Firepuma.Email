using Firepuma.Email.FunctionApp.Infrastructure.CommandHandling.PipelineBehaviors;
using Firepuma.Email.FunctionApp.Infrastructure.CommandHandling.TableProviders;
using Firepuma.Email.FunctionApp.Infrastructure.TableStorage;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Email.FunctionApp.Infrastructure.CommandHandling;

public static class ServiceCollectionExtensions
{
    public static void AddCommandHandling(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuditCommandsBehaviour<,>));

        services.AddTableProvider("EmailCommandExecutions", table => new CommandExecutionTableProvider(table));
    }
}