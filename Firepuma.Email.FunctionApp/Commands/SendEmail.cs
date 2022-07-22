using System.Threading;
using System.Threading.Tasks;
using Firepuma.Email.FunctionApp.Infrastructure.CommandHandling;
using Firepuma.Email.FunctionApp.Infrastructure.CommandHandling.TableModels.Attributes;
using Firepuma.Email.FunctionApp.Infrastructure.SendGridClient.Config;
using MediatR;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Local

namespace Firepuma.Email.FunctionApp.Commands;

public static class SendEmail
{
    public class Command : BaseCommand, IRequest<Result>
    {
        public string TemplateId { get; init; }

        [IgnoreCommandAudit]
        public object TemplateData { get; init; }

        public string Subject { get; init; }

        public string ToEmail { get; init; }
        public string ToName { get; init; }

        [IgnoreCommandAudit]
        public string HtmlBody { get; init; }

        [IgnoreCommandAudit]
        public string TextBody { get; init; }
    }

    public class Result
    {
        // Empty result for now
    }


    public class Handler : IRequestHandler<Command, Result>
    {
        private readonly IOptions<SendGridOptions> _sendGridOptions;
        private readonly ISendGridClient _sendGridClient;

        public Handler(
            ISendGridClient sendGridClient,
            IOptions<SendGridOptions> sendGridOptions)
        {
            _sendGridClient = sendGridClient;
            _sendGridOptions = sendGridOptions;
        }

        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var message = new SendGridMessage();

            message.SetFrom(_sendGridOptions.Value.FromEmailAddress);

            message.AddTo(command.ToEmail, command.ToName);
            message.SetSubject(command.Subject);

            if (!string.IsNullOrWhiteSpace(command.TemplateId))
            {
                message.SetTemplateId(command.TemplateId);
            }

            if (command.TemplateData != null)
            {
                message.SetTemplateData(command.TemplateData);
            }

            if (!string.IsNullOrWhiteSpace(command.HtmlBody))
            {
                message.AddContent("text/html", command.HtmlBody);
            }

            if (!string.IsNullOrWhiteSpace(command.TextBody))
            {
                message.AddContent("text/plain", command.TextBody);
            }

            await _sendGridClient.SendEmailAsync(message, cancellationToken);

            return new Result();
        }
    }
}