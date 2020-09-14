using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Configurations
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
                      options.Cookie.Name = "MvcPkceCookie";
                      //options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //this is diffferent than the access_token expiration
                      //options.SlidingExpiration = false;
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
                   //https://www.scottbrady91.com/OpenID-Connect/ASPNET-Core-using-Proof-Key-for-Code-Exchange-PKCE
                   options.UsePkce = true; // enable PKCE (authorization code flow only)

                   //recommended for best security. The response is transmitted via the HTTP POST method, 
                   //with the code or token being encoded in the body using the 
                   //application/x-www-form-urlencoded format.
                   //this allows us to keep codes out of the URL and protected via TLS
                   options.ResponseMode = "form_post";
                   options.UseTokenLifetime = true;
                   options.SaveTokens = true;
                   //options.GetClaimsFromUserInfoEndpoint = true;
                   options.Scope.Clear();
                   options.Scope.Add("openid");
                   //options.Scope.Add("profile");
                   //options.Scope.Add("inventory.read");
                   options.Scope.Add("otsuser");
                   //options.Scope.Add("invoices.write");
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
               });
            return services;
        }
    }
}