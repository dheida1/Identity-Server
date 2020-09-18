using IdentityModel;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SamlCore.AspNetCore.Authentication.Saml2.Metadata;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityServer.Api.Configurations
{
    public static class AuthenticationConfigs
    {
        public static IServiceCollection AddOtsAuthentication(
           this IServiceCollection services,
           IHostEnvironment environment,
           IConfiguration configuration)
        {
            services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                sharedOptions.DefaultSignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
            })
           .AddCertificate("x509", options =>
           {
               options.RevocationMode = (environment.IsDevelopment() ? X509RevocationMode.NoCheck : X509RevocationMode.Online);
               options.AllowedCertificateTypes = (environment.IsDevelopment() ? CertificateTypes.SelfSigned : CertificateTypes.Chained);
               options.ValidateCertificateUse = (environment.IsDevelopment() ? false : true);
               options.ValidateValidityPeriod = (environment.IsDevelopment() ? false : true);

               options.Events = new CertificateAuthenticationEvents
               {
                   OnCertificateValidated = context =>
                    {
                        context.Principal = Principal.CreateFromCertificate(context.ClientCertificate, includeAllClaims: true);
                        context.Success();
                        return Task.CompletedTask;
                    },
                   OnAuthenticationFailed = context =>
                    {
                        return Task.CompletedTask;
                    }
               };
           })
           .AddSamlCore("adfs", options =>
             {
                 options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                 options.ServiceProvider.EntityId = configuration["AppConfiguration:ServiceProvider:EntityId"];
                 options.MetadataAddress = configuration["AppConfiguration:IdentityProvider:MetadataAddress"];
                 options.SignOutPath = "/signedout";

                 options.RequireMessageSigned = false;
                 options.WantAssertionsSigned = true;
                 options.CreateMetadataFile = true;

                 if (environment.IsDevelopment())
                 {
                     options.ServiceProvider.X509Certificate2 = new X509Certificate2(@"Certificates/IdentityServer.pfx",
                         "1234", X509KeyStorageFlags.Exportable);
                 }
                 else
                 {
                     //if you want to search in cert store - can be used for production
                     options.ServiceProvider.X509Certificate2 = new Cryptography.X509Certificates.Extension.X509Certificate2(
                         configuration["AppConfiguration:ServiceProvider:Certificate"],
                         StoreName.My,
                         StoreLocation.LocalMachine,
                         X509FindType.FindBySerialNumber);
                 }
                 options.ForceAuthn = true;
                 options.WantAssertionsSigned = true;
                 options.RequireMessageSigned = false;

                 options.ServiceProvider.ServiceName = "My Test Site";
                 options.ServiceProvider.Language = "en-US";
                 options.ServiceProvider.OrganizationDisplayName = "Louisiana State Government";
                 options.ServiceProvider.OrganizationName = "Louisiana State Government";
                 options.ServiceProvider.OrganizationURL = "https://ots.la.gov";
                 options.ServiceProvider.ContactPerson = new ContactType()
                 {
                     Company = "Louisiana State Government - OTS",
                     GivenName = "Dina Heidar",
                     EmailAddress = new[] { "dina.heidar@la.gov" },
                     contactType = ContactTypeType.technical,
                     TelephoneNumber = new[] { "+1 234 5678" }
                 };
                 options.Events.OnRemoteFailure = context =>
                  {
                      return Task.FromResult(0);
                  };
                 options.Events.OnTicketReceived = context =>
                  {
                      var identity = context.Principal.Identity as ClaimsIdentity;
                      var claims = context.Principal.Claims;
                      var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                      identity.AddClaim(new Claim(ClaimTypes.Email, name));
                      return Task.FromResult(0);
                  };
             })
           .AddCookie();
            return services;
        }
    }
}