using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SharedServices.Core.Enums;
using SharedServices.Infrastructure.Authorization;

namespace SharedServices.Infrastructure.Extensions
{
    public static class AuthorizationServiceCollectionExtensions
    {
        public static IServiceCollection AddPermissionPolicies(this IServiceCollection services)
        {
            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddAuthorization(options =>
            {
                foreach (PermissionType permission in Enum.GetValues(typeof(PermissionType)))
                {
                    options.AddPolicy(permission.ToString(), policy =>
                        policy.Requirements.Add(new PermissionRequirement(permission)));
                }
            });

            return services;
        }
    }
}
