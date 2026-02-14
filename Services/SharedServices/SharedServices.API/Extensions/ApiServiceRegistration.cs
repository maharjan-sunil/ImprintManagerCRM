using Shared.Common.Models;
using SharedServices.Core.Entities;
using SharedServices.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using SharedServices.Application;
using SharedServices.Application.Extensions;
using SharedServices.Infrastructure.Extensions;
using static CoreUtilities.Auth.Extensions.JwtAuthExtensions;
using Email.Grpc;

namespace SharedServices.API.Extensions
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddApplicationServices();
            services.AddInfrastructureServices(config);
            services.AddPermissionPolicies();

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<SharedServiceDbContext>()
                .AddDefaultTokenProviders();

            services.AddJwtAuthentication(config);

            services.Configure<SmtpSettings>(
                config.GetSection("SmtpSettings"));

            services.AddGrpcClient<EmailService.EmailServiceClient>(o =>
            {
                o.Address = new Uri("http://localhost:8080"); // EmailService URL
            });

            services.AddScoped<Shared.GrpcClients.IEmailClientService, Shared.GrpcClients.EmailClientService>();

            services.AddOpenApi();

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly);
            });

            services.AddRouting(options => options.LowercaseUrls = true);
            return services;
        }
    }
}
