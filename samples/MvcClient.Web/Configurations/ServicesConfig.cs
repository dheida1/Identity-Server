using IdentityModel.Client;
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

            //    services.AddSingleton<IDiscoveryCache>(provider =>
            //    {
            //        var factory = provider.GetRequiredService<IHttpClientFactory>();
            //        return new DiscoveryCache(configuration["IdentityServer:Authority"], () => factory.CreateClient());
            //    });


            services.AddSingleton(new ClientCredentialsTokenRequest
            {
                Address = "http://localhost:5000/connect/token",
                ClientId = configuration["Client:Id"]
            });

            services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["IdentityServer:Authority"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<MtlsHandler>();

            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<BearerTokenHandler>() //order is important here
            .AddHttpMessageHandler<MtlsHandler>();

            return services;
        }
    }
}