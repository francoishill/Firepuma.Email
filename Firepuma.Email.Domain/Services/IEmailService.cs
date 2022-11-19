using Firepuma.Email.Domain.Models;

namespace Firepuma.Email.Domain.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken);
}