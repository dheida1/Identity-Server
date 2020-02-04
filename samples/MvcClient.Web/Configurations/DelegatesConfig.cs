using Microsoft.Extensions.DependencyInjection;
using MvcClient.Web.DelegatingHandlers;

namespace MvcClient.Web.Configurations
{
    public static class DelegatesConfig
    {
        public static IServiceCollection AddDelegates(
            this IServiceCollection services)
        {
            //to add the certicate to the http client header
            services.AddTransient<MtlsHandler>();

            //add bearer token to the http client header
            // services.AddTransient<BearerTokenHandler>();

            return services;
        }
    }
}
