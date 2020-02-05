using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using MvcClient.Web.DelegatingHandlers;
using System;
using System.Threading.Tasks;

namespace MvcClient.Web.Configurations
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
                      options.ExpireTimeSpan = TimeSpan.FromMinutes(60); //this is diffferent than the access_token expiration
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
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.BackchannelHttpHandler = new MtlsHandler(configuration, environment);
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access"); //need this to get back .refreshToken
                    options.Scope.Add("api1");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = "name",
                        RoleClaimType = "role"
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