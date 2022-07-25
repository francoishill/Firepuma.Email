using ExampleSendEmailApi.Controllers.Requests;
using Firepuma.Email.Abstractions.Models.Dtos.ServiceBusMessages;
using Firepuma.Email.Client.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExampleSendEmailApi.Controllers;

/// <summary>
/// API controller to send emails
/// </summary>
[ApiController]
[Route("[controller]")]
public class EmailController : ControllerBase
{
    private readonly ILogger<EmailController> _logger;
    private readonly IEmailEnqueuingClient _emailEnqueuingClient;

    public EmailController(
        ILogger<EmailController> logger,
        IEmailEnqueuingClient emailEnqueuingClient)
    {
        _logger = logger;
        _emailEnqueuingClient = emailEnqueuingClient;
    }

    /// <summary>
    /// Send an email
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> SendEmail(
        [FromBody] SendEmailRequest request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Using library to send an email (in background) via the Firepuma.Email.Client library");

        var emailRequest = new SendEmailRequestDto
        {
            ApplicationId = Constants.APPLICATION_ID,
            TemplateId = request.TemplateId,
            TemplateData = request.TemplateData,
            Subject = request.Subject,
            ToEmail = request.ToEmail,
            ToName = request.ToName,
            HtmlBody = request.HtmlBody,
            TextBody = request.TextBody,
        };

        var result = await _emailEnqueuingClient.EnqueueEmail(emailRequest, cancellationToken);

        if (!result.IsSuccessful)
        {
            return new BadRequestObjectResult(new
            {
                FailedReason = result.FailedReason.ToString(),
                Errors = result.FailedErrors,
            });
        }

        return Accepted();
    }
}