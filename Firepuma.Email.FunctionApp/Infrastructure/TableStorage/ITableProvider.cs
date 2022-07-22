using Microsoft.Azure.Cosmos.Table;

namespace Firepuma.Email.FunctionApp.Infrastructure.TableStorage;

public interface ITableProvider
{
    CloudTable Table { get; }
}