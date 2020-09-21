using IdentityServer.Api.Validators;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer.Infrastructure.Stores;
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
            //services.AddAutoMapper(typeof(ExtendedClientMapperProfile));
            services.AddTransient<IExtensionGrantValidator, DelegationGrantValidator>();
            //services.AddTransient<ITokenCreationService, JweTokenCreationService>();
            services.AddTransient(typeof(IPermissionStore<>), typeof(PermissionStore<>));
            return services;
        }
    }
}
