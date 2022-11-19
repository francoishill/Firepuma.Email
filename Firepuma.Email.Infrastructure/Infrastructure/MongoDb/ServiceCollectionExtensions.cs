using Firepuma.DatabaseRepositories.MongoDb;
using Firepuma.Email.Infrastructure.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Firepuma.Email.Infrastructure.Infrastructure.MongoDb;

public static class ServiceCollectionExtensions
{
    public static void AddMongoDbRepositories(
        this IServiceCollection services,
        IConfigurationSection mongoDbConfigSection,
        out MongoDbOptions mongoDbOptions)
    {
        if (mongoDbConfigSection == null) throw new ArgumentNullException(nameof(mongoDbConfigSection));

        services.AddOptions<MongoDbOptions>().Bind(mongoDbConfigSection).ValidateDataAnnotations().ValidateOnStart();
        mongoDbOptions = mongoDbConfigSection.Get<MongoDbOptions>()!;

        var tmpMongoDbOptions = mongoDbOptions;
        services.AddMongoDbRepositories(options =>
            {
                options.ConnectionString = tmpMongoDbOptions.ConnectionString;
                options.DatabaseName = tmpMongoDbOptions.DatabaseName;
            },
            validateOnStart: true);
    }
}