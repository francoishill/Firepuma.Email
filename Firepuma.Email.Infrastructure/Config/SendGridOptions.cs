namespace Firepuma.Email.Infrastructure.Config;

public class SendGridOptions
{
    public string ApiKey { get; set; } = null!;
    public string FromEmailAddress { get; set; } = null!;
}