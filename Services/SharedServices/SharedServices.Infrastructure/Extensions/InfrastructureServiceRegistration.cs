using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedServices.Application.Interfaces;
using SharedServices.Infrastructure.Authentication;
using SharedServices.Infrastructure.Authorization;
using SharedServices.Infrastructure.Persistence;

namespace SharedServices.Infrastructure.Extensions
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SharedServiceDbContext>(options =>
             options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                 b => b.MigrationsAssembly(typeof(SharedServiceDbContext).Assembly.FullName)));

            services.AddScoped<ISharedServiceDbContext, SharedServiceDbContext>();
            services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPermissionService, PermissionService>();

            return services;
        }
    }
}
