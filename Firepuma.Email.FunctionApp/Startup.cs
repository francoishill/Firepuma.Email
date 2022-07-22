using AutoMapper;
using Firepuma.Email.FunctionApp;
using Firepuma.Email.FunctionApp.Infrastructure.CommandHandling;
using Firepuma.Email.FunctionApp.Infrastructure.Helpers;
using Firepuma.Email.FunctionApp.Infrastructure.PipelineBehaviors;
using Firepuma.Email.FunctionApp.Infrastructure.SendGridClient;
using MediatR;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable RedundantTypeArgumentsOfMethod

[assembly: FunctionsStartup(typeof(Startup))]

namespace Firepuma.Email.FunctionApp;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var services = builder.Services;

        AddAutoMapper(services);
        AddMediator(services);
        AddCloudStorageAccount(services);

        services.AddCommandHandling();

        var apiKey = EnvironmentVariableHelpers.GetRequiredEnvironmentVariable("SendGridApiKey");
        var fromEmail = EnvironmentVariableHelpers.GetRequiredEnvironmentVariable("FromEmailAddress");

        services.AddSendGridClient(apiKey, fromEmail);
    }

    private static void AddMediator(IServiceCollection services)
    {
        services.AddMediatR(typeof(Startup));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceLogBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionLogBehavior<,>));
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));
        services.BuildServiceProvider().GetRequiredService<IMapper>().ConfigurationProvider.AssertConfigurationIsValid();
    }

    private static void AddCloudStorageAccount(IServiceCollection services)
    {
        var storageConnectionString = EnvironmentVariableHelpers.GetRequiredEnvironmentVariable("AzureWebJobsStorage");
        services.AddSingleton<CloudStorageAccount>(CloudStorageAccount.Parse(storageConnectionString));
    }
}