using Api2.Clients;
using Api2.DelegatingHandlers;
using Api2.Interfaces;
using Api2.Services;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api2.Configurations
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddDataServices(
              this IServiceCollection services,
              IConfiguration configuration)
        {
            services.AddTransient<BearerHandler>();

            services.AddSingleton(new TokenRequest
            {
                Address = configuration["IdentityServer:TokenEndpoint"],
                GrantType = "delegation",

                ClientId = configuration["Client:Id"],
                ClientAssertion = new ClientAssertion
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = TokenGenerator.CreateClientAuthJwt()
                },

                Parameters =
                {
                    { "scope", "api3 openid profile" }
                }
            });


            services.AddHttpClient<IIdentityServerClient, IdentityServerClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["IdentityServer:TokenEndpoint"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddUserAccessTokenHandler();

            // add automatic token management
            // this will refresh the mvc client access_token 
            //this has not been implemented here but this shows how it can be
            // this is different than user access_token
            services.AddAccessTokenManagement(o =>
                {
                    o.Client.Clients.Add("api2", new ClientCredentialsTokenRequest()
                    {
                        Address = configuration["IdentityServer:TokenEndpoint"],
                        ClientId = configuration["Client:Id"],
                        Scope = "api3",
                        ClientAssertion = new ClientAssertion
                        {
                            Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                            Value = TokenGenerator.CreateClientAuthJwt()
                        },
                    });
                })
            .ConfigureBackchannelHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            //create an api2 service to call the api3
            //this will get back an access_token with the sub as the user
            services.AddHttpClient<IApi3ServiceClient, Api3ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api3:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddHttpMessageHandler<BearerHandler>();

            return services;
        }
    }
}
