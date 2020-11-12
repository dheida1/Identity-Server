using IdentityModel;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcJwtClient.Web.Interfaces;
using MvcJwtClient.Web.Services;
using System;

namespace MvcJwtClient.Web.Configurations
{
    public static class TokenManagementServicesConfig
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
                //define the client-api that you wish to access
                //this will retireve an access_token specific to api1
                //this cannot be used for any other api's...it protects the other api's
                //machine to machine no user data. will not accept scope =openid since it is a 
                //client credentials token request
                o.Client.Clients.Add("api1", new ClientCredentialsTokenRequest()
                {
                    Address = configuration["IdentityServer:TokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "api1",
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


            //add all the api services 

            //create an api1 service to call the api (machine to machine)
            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
             {
                 client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                 client.DefaultRequestHeaders.Add("Accept", "application/json");
             })
             .AddClientAccessTokenHandler("api1");

            //create an api2 service to call the api2
            //this will get back an access_token with the user claims - on behalf of the user
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