using Firepuma.Email.Domain.Services;
using Firepuma.Email.Infrastructure.Config;
using Firepuma.Email.Infrastructure.Emailing.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SendGrid.Extensions.DependencyInjection;

namespace Firepuma.Email.Infrastructure.Emailing;

public static class ServiceCollectionExtensions
{
    public static void AddEmailing(
        this IServiceCollection services,
        IConfigurationSection sendGridConfigSection)
    {
        if (sendGridConfigSection == null) throw new ArgumentNullException(nameof(sendGridConfigSection));

        services.AddOptions<SendGridOptions>().Bind(sendGridConfigSection).ValidateDataAnnotations().ValidateOnStart();
        var sendGridOptions = sendGridConfigSection.Get<SendGridOptions>()!;

        services.AddSendGrid(o =>
        {
            o.ApiKey = sendGridOptions.ApiKey;
        });

        services.AddSingleton<IEmailService, SendGridEmailService>();
    }
}