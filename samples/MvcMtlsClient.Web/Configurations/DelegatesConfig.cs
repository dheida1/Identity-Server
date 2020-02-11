using Microsoft.Extensions.DependencyInjection;
using MvcMtlsClient.Web.DelegatingHandlers;

namespace MvcMtlsClient.Web.Configurations
{
    public static class DelegatesConfig
    {
        public static IServiceCollection AddDelegates(
            this IServiceCollection services)
        {
            //to add the certicate to the http client header
            services.AddTransient<MtlsHandler>();
            return services;
        }
    }
}
