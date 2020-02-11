using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcMtlsClient.Web.DelegatingHandlers;
using MvcMtlsClient.Web.Interfaces;
using MvcMtlsClient.Web.Services;
using System;

namespace MvcMtlsClient.Web.Configurations
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // add automatic token management
            // this will refresh the mvc client access_token and use it along with the mtls cert
            // when calling an api
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Scope = "api2.full_access";
                //define the client-api that you wish to access
                //this will retireve an access_token specific to api1
                //this cannot be used for any other api's...it protects the other api's
                o.Client.Clients.Add("api1", new ClientCredentialsTokenRequest()
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "api1",
                });
            })
            .ConfigureBackchannelHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler<MtlsHandler>();

            //create an api1 service to call the api
            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
             {
                 client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                 client.DefaultRequestHeaders.Add("Accept", "application/json");
             })
            //order below is very important here 
            //make sure you want the correct api1 since it'll ask ids to get the access_token with that specific scope(s)
            .AddClientAccessTokenHandler("api1")   //order is important here            
            .AddHttpMessageHandler<MtlsHandler>();

            //create an api2 service to call the api2
            services.AddHttpClient<IApi2ServiceClient, Api2ServiceClient>(client =>
                    {
                        client.BaseAddress = new Uri(configuration["Api2:BaseUrl"]);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    })
            .AddUserAccessTokenHandler()//order is important here       
            .AddHttpMessageHandler<MtlsHandler>();
            return services;
        }
    }
}