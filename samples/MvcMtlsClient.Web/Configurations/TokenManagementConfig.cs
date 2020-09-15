using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcMtlsClient.Web.DelegatingHandlers;
using MvcMtlsClient.Web.Interfaces;
using MvcMtlsClient.Web.Services;
using Polly;
using System;

namespace MvcMtlsClient.Web.Configurations
{
    public static class TokenManagementConfig
    {
        public static IServiceCollection AddTokenManagementServices(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            //generic
            services.AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("Invoices", new ClientCredentialsTokenRequest
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "invoices.read", // optional, 
                });
            })
                .ConfigureBackchannelHttpClient()
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                })
            )
                .AddHttpMessageHandler<MtlsHandler>();

            //api2
            services.AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("Inventory", new ClientCredentialsTokenRequest
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "inventory.read", // optional, 

                });
            })
                .ConfigureBackchannelHttpClient(client =>
                {
                    client.Timeout = TimeSpan.FromSeconds(30);
                })
                .AddTransientHttpErrorPolicy(policy => policy.WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(3)
                })
            );

            //Api services.


            //api1  - invoices
            //create an api1 service for the user to call the invoices api 
            //this will refresh the token if expired and 
            //attach the user access token (access_token with some user permissions as claims)"
            services.AddHttpClient<IApi1UserService, Api1UserService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddUserAccessTokenHandler()
            .AddHttpMessageHandler<MtlsHandler>();

            //api1  - invoices
            //create an api1 service for a batch job to call the invoices api  (eg hangfire)
            //this will refresh and attach the client access token (access_token with some scopes)"
            services.AddHttpClient<IApi1ClientService, Api1ClientService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddClientAccessTokenHandler("Invoices")
            .AddHttpMessageHandler<MtlsHandler>();



            return services;
        }
    }
}