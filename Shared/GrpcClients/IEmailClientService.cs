using Email.Grpc;

namespace Shared.GrpcClients
{
    public interface IEmailClientService
    {
        Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken ct = default);
        Task<SendEmailResponse> SendTemplateEmailAsync(SendTemplateEmailRequest request, CancellationToken ct = default);
    }
}
