using IdentityServer.Api.Services;
using IdentityServer.Api.Validators;
using IdentityServer.Infrastructure.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Hosting;
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
             IWebHostEnvironment environment,
             IConfiguration configuration)
        {
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
                options.EmitStaticAudienceClaim = true;

                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                //for mtls
                options.MutualTls.Enabled = true;
                options.MutualTls.ClientCertificateAuthenticationScheme = "x509";
            })
                 .AddAspNetIdentity<ApplicationUser>()
                 .AddJwtBearerClientAuthentication() //to accept clients via jwts

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

                 .AddProfileService<ProfileService>()
                 .AddExtensionGrantValidator<DelegationGrantValidator>();

            //for mtls
            identityServer.AddMutualTlsSecretValidators();

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