using Email.Grpc;
using EmailService.Interfaces;
using Grpc.Core;

namespace EmailService.Services
{
    public sealed class EmailGrpcService : Email.Grpc.EmailService.EmailServiceBase
    {
        private readonly IEmailSender _sender;
        private readonly ILogger<EmailGrpcService> _logger;

        public EmailGrpcService(IEmailSender sender, ILogger<EmailGrpcService> logger)
        {
            _sender = sender;
            _logger = logger;
        }

        public override async Task<SendEmailResponse> SendEmail(SendEmailRequest request, ServerCallContext context)
        {
            var (success, messageId, error) = await _sender.SendAsync(
                request.From,
                request.To,
                request.Cc,
                request.Bcc,
                request.Subject,
                request.HtmlBody,
                request.TextBody,
                request.Attachments.Select(a => (a.FileName, a.Content.ToByteArray(), a.ContentType ?? null)).ToList(),
                request.Headers.Select(h => (h.Key, h.Value)).ToList(),
                context.CancellationToken);

            return new SendEmailResponse
            {
                Success = success,
                MessageId = messageId ?? string.Empty,
                Error = error ?? string.Empty
            };
        }

        public override async Task<SendEmailResponse> SendTemplateEmail(SendTemplateEmailRequest request, ServerCallContext context)
        {
            var (success, messageId, error) = await _sender.SendTemplateAsync(
                request.TemplateName,
                request.Data,
                request.To,
                string.IsNullOrWhiteSpace(request.Subject) ? null : request.Subject,
                context.CancellationToken);

            return new SendEmailResponse
            {
                Success = success,
                MessageId = messageId ?? string.Empty,
                Error = error ?? string.Empty
            };
        }
    }
}
