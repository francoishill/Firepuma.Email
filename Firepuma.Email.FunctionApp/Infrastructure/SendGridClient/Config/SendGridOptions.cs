namespace Firepuma.Email.FunctionApp.Infrastructure.SendGridClient.Config;

public class SendGridOptions
{
    public string ApiKey { get; set; }
    public string FromEmailAddress { get; set; }
}