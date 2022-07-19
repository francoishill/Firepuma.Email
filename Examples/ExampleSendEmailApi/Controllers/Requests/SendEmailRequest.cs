using System.ComponentModel.DataAnnotations;

namespace ExampleSendEmailApi.Controllers.Requests;

public class SendEmailRequest
{
    public string TemplateId { get; set; }
    public object TemplateData { get; set; }

    [Required]
    public string Subject { get; set; }

    [Required]
    public string ToEmail { get; set; }

    public string ToName { get; set; }

    public string HtmlBody { get; set; }

    [Required]
    public string TextBody { get; set; }
}