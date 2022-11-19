using Firepuma.CommandsAndQueries.Abstractions.Commands;
using Firepuma.CommandsAndQueries.Abstractions.Entities.Attributes;
using Firepuma.Email.Domain.Models;
using Firepuma.Email.Domain.Services;
using MediatR;

#pragma warning disable CS8618

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local
// ReSharper disable RedundantAssignment
// ReSharper disable UnusedType.Global
// ReSharper disable ClassNeverInstantiated.Global

namespace Firepuma.Email.Domain.Commands;

public static class SendEmailCommand
{
    public class Payload : BaseCommand<Result>
    {
        public required string ApplicationId { get; init; }

        public required string? TemplateId { get; init; }

        [IgnoreCommandExecution]
        public required object? TemplateData { get; init; }

        public required string Subject { get; init; }

        public required string ToEmail { get; init; }
        public required string? ToName { get; init; }

        [IgnoreCommandExecution]
        public required string? HtmlBody { get; init; }

        [IgnoreCommandExecution]
        public required string TextBody { get; init; }
    }

    public class Result
    {
    }

    public class Handler : IRequestHandler<Payload, Result>
    {
        private readonly IEmailService _emailService;

        public Handler(
            IEmailService emailService)
        {
            _emailService = emailService;
        }

        public async Task<Result> Handle(
            Payload payload,
            CancellationToken cancellationToken)
        {
            var message = new EmailMessage
            {
                TemplateId = payload.TemplateId,
                TemplateData = payload.TemplateData,
                Subject = payload.Subject,
                ToEmail = payload.ToEmail,
                ToName = payload.ToName,
                HtmlBody = payload.HtmlBody,
                TextBody = payload.TextBody,
            };

            await _emailService.SendEmailAsync(message, cancellationToken);

            return new Result();
        }
    }
}