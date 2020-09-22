using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MvcMtlsClient.Web.DelegatingHandlers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Configurations
{
    public static class AuthenticationConfig
    {
        public static IServiceCollection AddOtsAuthentication(
            this IServiceCollection services,
            IWebHostEnvironment environment,
            IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                  {
                      options.Cookie.Name = "MvcMtlsCookie";
                      options.SlidingExpiration = false;
                      options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                      options.Events.OnRedirectToAccessDenied = context =>
                      {
                          return Task.CompletedTask;
                      };
                      options.Events.OnSigningOut = async e =>
                      {
                          // automatically revoke refresh token at signout time
                          await e.HttpContext.RevokeUserRefreshTokenAsync();
                      };
                  })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = configuration["IdentityServer:Authority"];
                    options.RequireHttpsMetadata = environment.IsDevelopment() ? false : true;
                    options.ClientId = configuration["Client:Id"];
                    options.ResponseType = "code";
                    options.SaveTokens = true;

                    options.BackchannelHttpHandler = new MtlsHandler(configuration, environment);
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("invoices");
                    options.Scope.Add("inventory");
                    options.Scope.Add("offline_access"); //need this to get back '.refreshToken' to use when calling api's   

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.PreferredUserName,
                        RoleClaimType = "role",
                        TokenDecryptionKey =
                       environment.IsDevelopment() ? new X509SecurityKey(new X509Certificate2("Certificates/MvcClient.Web.pfx", "1234"))
                       : new X509SecurityKey(new Cryptography.X509Certificates.Extension.X509Certificate2(
                        configuration["Certificates:Personal"],
                        StoreName.My,
                        StoreLocation.LocalMachine,
                        X509FindType.FindBySerialNumber))
                    };

                    options.Events.OnRedirectToIdentityProvider = async context =>
                    {
                        var config = await context.Options.ConfigurationManager.GetConfigurationAsync(default);
                        //override existing token endpoint with to get the mtls token
                        config.TokenEndpoint = configuration["IdentityServer:MtlsTokenEndpoint"];
                    };
                });
            return services;
        }
    }
}