namespace EmailService.Interfaces
{
    public interface IEmailSender
    {
        Task<(bool Success, string? MessageId, string? Error)> SendAsync(
            string? from,
            IEnumerable<string> to,
            IEnumerable<string>? cc,
            IEnumerable<string>? bcc,
            string subject,
            string? html,
            string? text,
            IEnumerable<(string FileName, byte[] Content, string? ContentType)>? attachments,
            IEnumerable<(string Key, string Value)>? headers,
            CancellationToken ct);

        Task<(bool Success, string? MessageId, string? Error)> SendTemplateAsync(
            string templateName,
            IDictionary<string, string> data,
            string to,
            string? subjectOverride,
            CancellationToken ct);

        Task<bool> CallFromDemo();
    }
}
