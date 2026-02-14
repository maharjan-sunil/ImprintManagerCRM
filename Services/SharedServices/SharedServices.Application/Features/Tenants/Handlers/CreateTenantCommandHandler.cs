using CoreUtilities.Security;
using Email.Grpc;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.Common.Constants;
using Shared.Common.Enums;
using Shared.Common.Extensions;
using Shared.Common.Models;
using Shared.GrpcClients;
using SharedServices.Application.Features.Tenants.Commands;
using SharedServices.Application.Interfaces;
using SharedServices.Core.Entities;

namespace SharedServices.Application.Features.Tenants.Handlers
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, Result<long>>
    {
        private readonly ISharedServiceDbContext _sharedServiceDbContext;
        private readonly IEmailClientService _emailService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly string _clientAppUrl;

        public CreateTenantCommandHandler(ISharedServiceDbContext sharedServiceDbContext, IEmailClientService emailService, UserManager<User> userManager, RoleManager<Role> roleManager, IConfiguration config)
        {
            _sharedServiceDbContext = sharedServiceDbContext;
            _emailService = emailService;
            _userManager = userManager;
            _roleManager = roleManager;
            _clientAppUrl = config["ClientAppUrl"]!;
        }

        public async Task<Result<long>> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var tenant = new Tenant
                {
                    TenantName = command.TenantName,
                    TenantCode = command.TenantCode,
                    Email = command.Email,
                    SubscriptionTier = command.SubscriptionTier,
                    MaxLocations = command.MaxLocations,
                    IsActive = command.IsActive
                };

                _sharedServiceDbContext.Tenants.Add(tenant);
                await _sharedServiceDbContext.SaveChangesAsync(cancellationToken);

                string temporaryPassword = UrlSafePasswordGenerator.Generate();

                var user = new User
                {
                    TenantId = tenant.TenantId,
                    UserName = command.Email,
                    Email = command.Email,
                    FirstName = command.TenantName,
                    LastName = command.TenantName,
                    IsActive = true
                };

                var result = await _userManager.CreateAsync(user, temporaryPassword);

                if (!await _roleManager.RoleExistsAsync("Tenant"))
                {
                    await _roleManager.CreateAsync(new Role
                    {
                        Name = "Tenant",
                        IsActive = true,
                    });
                }

                await _userManager.AddToRoleAsync(user, "Tenant");

                #region send email
                string loginUrl = $"{_clientAppUrl}/login";

                var emailRequest = new SendTemplateEmailRequest
                {
                    To = command.Email,
                    Subject = "Welcome to Imprint CRM!",
                    TemplateName = EmailTemplates.WelcomeUser,
                };

                emailRequest.Data.Add("Username", command.TenantName);
                emailRequest.Data.Add("TemporaryPassword", temporaryPassword);
                emailRequest.Data.Add("LoginUrl", loginUrl);

                await _emailService.SendTemplateEmailAsync(emailRequest, cancellationToken);
                #endregion

                return Result.Ok(tenant.TenantId).WithSuccess("Tenant created successfully.");
            }
            catch (Exception ex)
            {
                //logging
            }

            return Result.Fail(ResultHelper.WithErrorCode("Failed to create tenant.", ErrorCode.InternalError));
        }
    }
}
