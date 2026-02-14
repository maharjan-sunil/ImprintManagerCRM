using Email.Grpc;
using Grpc.Core;

namespace Shared.GrpcClients
{
    public class EmailClientService : IEmailClientService
    {
        private readonly EmailService.EmailServiceClient _client;

        public EmailClientService(EmailService.EmailServiceClient client)
        {
            _client = client;
        }

        public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken ct = default)
        {
            try
            {
                var headers = new Metadata
                {
                    { "x-correlation-id", Guid.NewGuid().ToString() }
                };

                return await _client.SendEmailAsync(request, headers: headers, cancellationToken: ct);
            }
            catch (RpcException ex)
            {
                return new SendEmailResponse { Success = false, Error = ex.Status.Detail };
            }
        }


        public async Task<SendEmailResponse> SendTemplateEmailAsync(SendTemplateEmailRequest request, CancellationToken ct = default)
        {
            try
            {
                var headers = new Metadata
                {
                    { "x-correlation-id", Guid.NewGuid().ToString() }
                };

                return await _client.SendTemplateEmailAsync(request, headers: headers, cancellationToken: ct);
            }
            catch (RpcException ex)
            {
                return new SendEmailResponse { Success = false, Error = ex.Status.Detail };
            }
        }
    }
}
