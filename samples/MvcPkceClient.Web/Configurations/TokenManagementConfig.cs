using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcPkceClient.Web.Interfaces;
using MvcPkceClient.Web.Services;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http;

namespace MvcPkceClient.Web.Configurations
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
                    Address = configuration["IdentityServer:TokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "invoices.read", // optional, 
                });
                options.Client.Clients.Add("Inventory", new ClientCredentialsTokenRequest
                {
                    Address = configuration["IdentityServer:TokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "inventory.read", // optional,                     
                });
            })
                .ConfigureBackchannelHttpClient()
                .AddPolicyHandler(GetRetryPolicy());
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
            .AddUserAccessTokenHandler();

            //api1  - invoices
            //create an api1 service for a batch job to call the invoices api  (eg hangfire)
            //this will refresh and attach the client access token (access_token with some scopes)"
            services.AddHttpClient<IApi1ClientService, Api1ClientService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddClientAccessTokenHandler("Invoices");


            //create an api2 service to call the invoices api2
            services.AddHttpClient<IApi2UserService, Api2UserService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api2:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddUserAccessTokenHandler();

            services.AddHttpClient<IApi2ClientService, Api2ClientService>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api2:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
           .AddClientAccessTokenHandler("Inventory");

            return services;
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              // Handle HttpRequestExceptions, 408 and 5xx status codes
              .HandleTransientHttpError()
              // Handle 404 not found
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              // Handle 401 Unauthorized
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.Unauthorized)
              // What to do if any of the above erros occur:
              // Retry 3 times, each time wait 1,2 and 4 seconds before retrying.
              .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}