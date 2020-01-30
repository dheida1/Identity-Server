using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcClient.Web.DelegatingHandlers;
using MvcClient.Web.Interfaces;
using MvcClient.Web.Services;
using System;

namespace MvcClient.Web.Configurations
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHttpClient<IApi1Service, Api1Service>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<BearerTokenHandler>() //order is imporatant here
            .AddHttpMessageHandler<MtlsHandler>();

            return services;
        }
    }
}
