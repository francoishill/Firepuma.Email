﻿using System.Text.Json;
using Firepuma.Email.Domain.Models;
using Firepuma.Email.Domain.Services;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Firepuma.Email.Infrastructure.Emailing.Services;

public class SendGridEmailService : IEmailService
{
    private readonly ILogger<SendGridEmailService> _logger;
    private readonly ISendGridClient _sendGridClient;

    public SendGridEmailService(
        ILogger<SendGridEmailService> logger,
        ISendGridClient sendGridClient)
    {
        _logger = logger;
        _sendGridClient = sendGridClient;
    }

    public async Task SendEmailAsync(EmailMessage message, CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Sending SendGrid email from email message payload: {Payload}",
            JsonSerializer.Serialize(message));

        var sendGridMessage = new SendGridMessage();

        sendGridMessage.SetFrom(message.FromEmail);

        sendGridMessage.AddTo(message.ToEmail, message.ToName);
        sendGridMessage.SetSubject(message.Subject);

        if (!string.IsNullOrWhiteSpace(message.TemplateId))
        {
            sendGridMessage.SetTemplateId(message.TemplateId);
        }

        if (message.TemplateData != null)
        {
            sendGridMessage.SetTemplateData(message.TemplateData);
        }

        if (!string.IsNullOrWhiteSpace(message.HtmlBody))
        {
            sendGridMessage.AddContent("text/html", message.HtmlBody);
        }

        if (!string.IsNullOrWhiteSpace(message.TextBody))
        {
            sendGridMessage.AddContent("text/plain", message.TextBody);
        }

        if (message.GroupId != null)
        {
            sendGridMessage.SetAsm(message.GroupId.Value, message.GroupsToDisplay);
        }

        var response = await _sendGridClient.SendEmailAsync(sendGridMessage, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var responseBody = "[NO BODY]";

            try
            {
                responseBody = await response.Body.ReadAsStringAsync(cancellationToken);
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception, "Unable to read body from response of SendGrid SendEmailAsync call, continuing to throw exception without body content");
            }

            _logger.LogError(
                "SendGrid SendEmailAsync call was unsuccessful (status code {Code}), response body: {Body}",
                response.StatusCode.ToString(), responseBody);

            throw new Exception($"Response was not successful for SendGrid SendEmailAsync call, response body: {responseBody}");
        }

        _logger.LogInformation("Email successfully sent to {To} with Subject '{Subject}'", message.ToEmail, message.Subject);
    }
}