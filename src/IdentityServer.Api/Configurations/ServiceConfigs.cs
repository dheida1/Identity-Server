using IdentityServer.Api.DependencyInjection;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Api.Configurations
{
    public static class ServiceConfigs
    {
        public static IServiceCollection AddDataServices(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddTransient<IProfileService, ProfileService>();
            services.AddTransient<IExtensionGrantValidator, DelegationGrantValidator>();
            return services;
        }
    }
}
