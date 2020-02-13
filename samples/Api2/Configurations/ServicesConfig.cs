using Api2.Interfaces;
using Api2.Services;
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
            // add automatic token management
            // this will refresh the mvc client access_token and use it along with the mtls cert
            // when calling an api
            services.AddAccessTokenManagement()
            .ConfigureBackchannelHttpClient(client =>
            {
                client.Timeout = TimeSpan.FromSeconds(30);
            });

            //create an api2 service to call the api2
            //this will get back an access_token with the user claims - on behalf of the user
            services.AddHttpClient<IApi3ServiceClient, Api3ServiceClient>(client =>
            {
                client.BaseAddress = new Uri(configuration["Api3:BaseUrl"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .AddUserAccessTokenHandler();
            return services;
        }
    }
}
