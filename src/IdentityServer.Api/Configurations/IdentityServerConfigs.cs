using IdentityServer.Api.Services;
using IdentityServer.Api.Validators;
using IdentityServer.Infrastructure.Data;
using IdentityServer.Infrastructure.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Security.Cryptography.X509Certificates;

namespace IdentityServer.Api.Configurations
{
    public static class IdentityServerConfigs
    {
        public static IServiceCollection AddIdentityServerConfigs(
             this IServiceCollection services,
             IHostEnvironment environment,
             IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, ApplicationRole>
                (options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            //for mtls
            services.AddCertificateForwarding(options =>
            {
                options.CertificateHeader = "X-ARR-ClientCert";
                options.HeaderConverter = (headerValue) =>
                {
                    X509Certificate2 clientCertificate = null;
                    if (!string.IsNullOrWhiteSpace(headerValue))
                    {
                        byte[] bytes = Convert.FromBase64String(headerValue);
                        clientCertificate = new X509Certificate2(bytes);
                    }
                    return clientCertificate;
                };
            });

            var migrationsAssembly = "IdentityServer.Infrastructure";
            var identityServer = services.AddIdentityServer(options =>
            {
                //disable out-of-the-box userinfo json endpoint and enable jwe response               
                options.AddUserInfoAsJwe();

                //add audience claim to the access_token
                options.EmitStaticAudienceClaim = true;

                options.IssuerUri = configuration["AppConfiguration:IdentityServer:Authority"];

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                //for mtls
                options.MutualTls.Enabled = true;
                options.MutualTls.ClientCertificateAuthenticationScheme = "x509";
            })
                 //add aspnet identity tables instead of creating your own 
                 //or using out-of-the-box test users
                 .AddAspNetIdentity<ApplicationUser>()

                 //to accept clients authentication using jwts 
                 .AddJwtBearerClientAuthentication()

                // this adds the config data from DB (clients, resources, CORS)                   
                .AddConfigurationStore<ConfigurationDbContext>(options =>
                {
                    options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                })
                // this adds the operational data from DB (codes, tokens, consents)
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = builder =>
                            builder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                            sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));

                    // this enables automatic token cleanup. this is optional.
                    options.EnableTokenCleanup = true;
                    options.TokenCleanupInterval = 3600; // interval in seconds (default is 3600)
                })

                 //to add custom profile service which collects info to 
                 //be added in access_tokens, id_token and userinfo
                 .AddProfileService<ProfileService>()

                 //add extension delegation capability (between api1->api2)
                 .AddExtensionGrantValidator<DelegationGrantValidator>();

            //for mtls
            identityServer.AddMutualTlsSecretValidators();

            //add the endpoints and services required to 
            //create all the user info jwe 
            identityServer.AddUserInfoAsJwe();

            if (environment.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                //TODO to comply with FAPI2 (Financial-grade API) get cert using ES256 and PS256 algorithm
                //https://www.scottbrady91.com/OpenSSL/Creating-Elliptical-Curve-Keys-using-OpenSSL
                //https://www.scottbrady91.com/Identity-Server/Using-ECDSA-in-IdentityServer4

                //identityServer.AddSigningCredential(new X509Certificate2(configuration["AppConfiguration:ServiceProvider:Certificate"], "1234"), signingAlgorithm: "ES256");
                throw new Exception("need to configure key material");
            }
            return services;
        }
    }
}