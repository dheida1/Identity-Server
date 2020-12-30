using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer.Api.Configurations
{
    public static class IISConfigs
    {
        public static IServiceCollection AddIISConfigs(
             this IServiceCollection services)
        {
            // configures IIS out-of-proc settings 
            //(see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
                     {
                         iis.AuthenticationDisplayName = "Windows";
                         iis.AutomaticAuthentication = false;
                     });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
            });
            return services;
        }
    }
}
