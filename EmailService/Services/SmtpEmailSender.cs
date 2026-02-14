using EmailService.Common.Extensions;
using EmailService.Interfaces;
using EmailService.Options;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmailService.Services
{
    public sealed class SmtpEmailSender(IOptions<SmtpSettingOptions> options, ILogger<SmtpEmailSender> logger, IEmailTemplateRenderer emailTemplateRenderer)
        : IEmailSender
    {
        private readonly SmtpSettingOptions _opt = options.Value;
        private readonly ILogger<SmtpEmailSender> _logger = logger;
        private readonly IEmailTemplateRenderer _emailTemplateRenderer = emailTemplateRenderer;

        public async Task<(bool Success, string? MessageId, string? Error)> SendAsync(
            string? from,
            IEnumerable<string> to,
            IEnumerable<string>? cc,
            IEnumerable<string>? bcc,
            string subject,
            string? html,
            string? text,
            IEnumerable<(string FileName, byte[] Content, string? ContentType)>? attachments,
            IEnumerable<(string Key, string Value)>? headers,
            CancellationToken ct)
        {
            try
            {
                var message = new MimeMessage();

                var fromAddress = new MailboxAddress(_opt.DisplayName, string.IsNullOrWhiteSpace(from) ? _opt.DefaultFrom : from);
                message.From.Add(fromAddress);

                foreach (var t in to) message.To.Add(MailboxAddress.Parse(t));
                if (cc != null) foreach (var c in cc) message.Cc.Add(MailboxAddress.Parse(c));
                if (bcc != null) foreach (var b in bcc) message.Bcc.Add(MailboxAddress.Parse(b));

                message.Subject = subject;

                var bodyBuilder = new BodyBuilder();
                if (!string.IsNullOrWhiteSpace(html)) bodyBuilder.HtmlBody = html;
                if (!string.IsNullOrWhiteSpace(text)) bodyBuilder.TextBody = text;

                if (attachments != null)
                {
                    foreach (var att in attachments)
                    {
                        var ctype = string.IsNullOrWhiteSpace(att.ContentType) ? "application/octet-stream" : att.ContentType;
                        bodyBuilder.Attachments.Add(att.FileName, att.Content, ContentType.Parse(ctype));
                    }
                }

                message.Body = bodyBuilder.ToMessageBody();

                if (headers != null)
                {
                    foreach (var (k, v) in headers)
                    {
                        message.Headers.Add(k, v);
                    }
                }

                using var smtp = new SmtpClient();
                SecureSocketOptions sec = _opt.UseSSL ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;

                await smtp.ConnectAsync(_opt.Host, _opt.Port, sec, ct);
                if (!string.IsNullOrEmpty(_opt.Username))
                {
                    await smtp.AuthenticateAsync(_opt.Username, _opt.Password, ct);
                }

                var response = await smtp.SendAsync(message, ct);
                await smtp.DisconnectAsync(true, ct);

                _logger.LogInformation("Email sent to {To}. SmtpResponse={Response}", string.Join(",", to), response);
                return (true, message.MessageId, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool Success, string? MessageId, string? Error)> SendTemplateAsync(
            string templateName,
            IDictionary<string, string> data,
            string to,
            string? subjectOverride,
            CancellationToken ct)
        {
            var subject = subjectOverride ?? $"Template: {templateName}";

            var modelType = EmailTemplateRegistry.GetModelType(templateName);
            if (modelType == null)
                throw new ArgumentException($"Unknown template: {templateName}");

            // Map dictionary to model
            var model = DictionaryMapper.MapToModel(data, modelType);

            // Render Razor template
            string htmlBody = await _emailTemplateRenderer.RenderAsync(templateName, model);

            return await SendAsync(null, new[] { to }, null, null, subject, htmlBody, null, null, null, ct);
        }
    }
}
