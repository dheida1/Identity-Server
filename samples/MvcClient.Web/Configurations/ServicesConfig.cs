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

            //services.AddTransient(serviceProvider =>
            //{
            //    var handler = new SocketsHttpHandler();
            //    handler.SslOptions.ClientCertificates =
            //        new X509CertificateCollection { X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single() };
            //    return handler;
            //});

            //services.AddSingleton<IDiscoveryCache>(provider =>
            //    {
            //        var factory = provider.GetRequiredService<IHttpClientFactory>();
            //        return new DiscoveryCache(configuration["IdentityServer:Authority"], () => factory.CreateClient().);
            //    });


            // add automatic token management
            // this will refresh the mvc client access_token and use it along with the mtls cert
            // when calling an api
            services.AddAccessTokenManagement(o =>
            {
                //define the client-api that you wish to access
                //this will retireve an access_token specific to api1
                //this cannot be used for any other api's...it protects the other api's
                o.Client.Clients.Add("api1", new ClientCredentialsTokenRequest()
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "api1",
                });
                o.Client.Clients.Add("api2", new ClientCredentialsTokenRequest()
                {
                    Address = configuration["IdentityServer:MtlsTokenEndpoint"],
                    ClientId = configuration["Client:Id"],
                    Scope = "api2"
                });
            })
        .ConfigureBackchannelHttpClient(client =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
        })
        .AddHttpMessageHandler<MtlsHandler>();

            //    new X509CertificateCol = lection { X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single() };
            //.AddHttpMessageHandler<MtlsHandler>();

            //.ConfigureHttpMessageHandlerBuilder((c) =>
            //{
            //    var handler = new SocketsHttpHandler();
            //    handler.SslOptions.ClientCertificates =
            //        new X509CertificateCollection { X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single() };
            //});
            //.ConfigurePrimaryHttpMessageHandler(() =>
            // {
            //     var handler = new SocketsHttpHandler();
            //handler.SslOptions.ClientCertificates =
            //    new X509CertificateCollection { X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single() };
            //return handler;
            //});
            //.ConfigurePrimaryHttpMessageHandler(sp => sp.GetRequiredService<SocketsHttpHandler>());



            //create an api1 service to call the api
            services.AddHttpClient<IApi1ServiceClient, Api1ServiceClient>(client =>
             {
                 client.BaseAddress = new Uri(configuration["Api1:BaseUrl"]);
                 client.DefaultRequestHeaders.Add("Accept", "application/json");
             })
             //order below is very important here 
             //make sure you want the correct api1 since it'll ask ids to get the access_token with that specific scope(s)
             .AddClientAccessTokenHandler("api1");
            //.AddHttpMessageHandler<MtlsHandler>();

            //create an api2 service to call the api2
            services.AddHttpClient<IApi2ServiceClient, Api2ServiceClient>(client =>
                    {
                        client.BaseAddress = new Uri(configuration["Api2:BaseUrl"]);
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    })
                    .AddClientAccessTokenHandler("api2");//order is important here       
            //.AddHttpMessageHandler<MtlsHandler>();
            return services;
        }
    }
}