using Firepuma.Email.FunctionApp.Infrastructure.SendGridClient.Config;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SendGrid.Extensions.DependencyInjection;

namespace Firepuma.Email.FunctionApp.Infrastructure.SendGridClient;

public static class ServiceCollectionExtensions
{
    public static void AddSendGridClient(
        this IServiceCollection services,
        string apiKey,
        string fromEmail)
    {
        services.Configure<SendGridOptions>(opt =>
        {
            opt.ApiKey = apiKey;
            opt.FromEmailAddress = fromEmail;
        });

        services.AddSendGrid((s, options) =>
        {
            var config = s.GetRequiredService<IOptions<SendGridOptions>>();

            options.ApiKey = config.Value.ApiKey;
        });
    }
}