using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Azure.WebJobs;
using SendGrid.Helpers.Mail;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Local

namespace Firepuma.Email.FunctionApp.Commands;

public static class SendEmail
{
    public class Command : IRequest<Result>
    {
        public IAsyncCollector<SendGridMessage> EmailMessageCollector { get; init; }

        public string TemplateId { get; init; }
        public object TemplateData { get; init; }

        public string Subject { get; init; }

        public string ToEmail { get; init; }
        public string ToName { get; init; }

        public string HtmlBody { get; init; }
        public string TextBody { get; init; }
    }

    public class Result
    {
        // Empty result for now
    }


    public class Handler : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var emailMessageCollector = command.EmailMessageCollector;

            var message = new SendGridMessage();

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


            await emailMessageCollector.AddAsync(message, cancellationToken);
            await emailMessageCollector.FlushAsync(cancellationToken);


            return new Result();
        }
    }
}