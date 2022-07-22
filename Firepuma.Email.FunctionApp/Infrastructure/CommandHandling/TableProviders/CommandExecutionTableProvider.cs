using Firepuma.Email.FunctionApp.Infrastructure.TableStorage;
using Microsoft.Azure.Cosmos.Table;

namespace Firepuma.Email.FunctionApp.Infrastructure.CommandHandling.TableProviders;

public class CommandExecutionTableProvider : ITableProvider
{
    public CloudTable Table { get; }

    public CommandExecutionTableProvider(CloudTable table)
    {
        Table = table;
    }
}