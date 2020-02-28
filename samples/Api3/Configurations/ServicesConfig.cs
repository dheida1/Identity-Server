using Api3.Interfaces;
using Api3.Services;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Api3.Configurations
{
    public static class ServicesConfig
    {
        public static IServiceCollection AddDataServices(
              this IServiceCollection services,
              IConfiguration configuration)
        {
            services.AddAccessTokenManagement(o =>
            {
                o.Client.Clients.Add("api3", new ClientCredentialsTokenRequest()
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

            services.AddSingleton(new TokenIntrospectionRequest
            {
                Address = configuration["IdentityServer:Introspection"],
                ClientId = configuration["Client:Id"],
                ClientAssertion = new ClientAssertion
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = TokenGenerator.CreateClientAuthJwt()
                },
            });

            services.AddHttpClient<IIntrospectClient, IntrospectClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["IdentityServer:Authority"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddClientAccessTokenHandler();

            return services;
        }
    }
}
