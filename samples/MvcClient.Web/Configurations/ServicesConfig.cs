using IdentityModel.AspNetCore.AccessTokenManagement;
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
            //services.AddSingleton<IDiscoveryCache>(provider =>
            //{
            //    var factory = provider.GetRequiredService<IHttpClientFactory>();
            //    return new DiscoveryCache(configuration["IdentityServer:Authority"], () => factory.CreateClient());
            //});

            //services.AddSingleton(new IdentityModel.Client.TokenRequest
            //{
            //    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
            //    ClientId = configuration["Client:Id"],
            //    GrantType = GrantTypes.AuthorizationCode
            //});

            //services.AddSingleton(new ClientCredentialsTokenRequest
            //{
            //    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
            //    ClientId = configuration["Client:Id"]
            //});

            //services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration["IdentityServer:Authority"]);
            //    client.DefaultRequestHeaders.Add("Accept", "application/json");
            //})
            //.AddHttpMessageHandler<MtlsHandler>();

            // add HTTP client to call protected API
            //services.AddUserAccessTokenClient("client", client =>
            //{
            //    client.BaseAddress = new Uri(Constants.SampleApi);
            //});

            services.AddHttpClient<ITokenEndpointService, TokenEndpointService>();

            // add automatic token management
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Clients.Add("mvcCert", new ClientCredentialsTokenRequest()
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    //Scope = "offline_access api1 openid",
                    //GrantType = GrantTypes.AuthorizationCode
                });
            }).ConfigureBackchannelHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(30);

                }).AddHttpMessageHandler<MtlsHandler>();

            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            //.AddHttpMessageHandler<BearerTokenHandler>() //order is important here          
            .AddClientAccessTokenHandler()
            .AddHttpMessageHandler<MtlsHandler>();

            return services;
        }
    }
}