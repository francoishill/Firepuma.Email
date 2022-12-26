namespace Firepuma.Email.Domain.Models;

public class EmailMessage
{
    public required string? TemplateId { get; init; }
    public required Dictionary<string, string>? TemplateData { get; init; }
    public required string Subject { get; init; }
    public required string FromEmail { get; init; }
    public required string ToEmail { get; init; }
    public required string? ToName { get; init; }
    public required string? HtmlBody { get; init; }
    public required string TextBody { get; init; }
    public required int? GroupId { get; init; }
    public required List<int>? GroupsToDisplay { get; init; }
}