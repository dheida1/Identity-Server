﻿using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcPkceClient.Web.Interfaces;
using MvcPkceClient.Web.Services;
using Polly;
using System;

namespace MvcPkceClient.Web.Configurations
{
    public static class TokenManagementConfig
    {
        public static IServiceCollection AddTokenManagementServices(
           this IServiceCollection services,
           IConfiguration configuration)
        {
            services.AddAccessTokenManagement(options =>
            {
                options.Client.Clients.Add("identityserver", new ClientCredentialsTokenRequest
                {
                    Address = configuration["IdentityServer:TokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    //Scope = "inventory.read" // optional
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

            //create an api1 service to call the inventory api 
            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
             .AddClientAccessTokenHandler("identityserver");


            //create an api2 service to call the invoices api2
            services.AddHttpClient<IApi2ServiceClient, Api2ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api2:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddUserAccessTokenHandler();

            return services;
        }
    }
}