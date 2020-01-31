using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                      options.SlidingExpiration = true;
                      options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                      options.Events.OnValidatePrincipal = context =>
                      {
                          return Task.CompletedTask;
                      };
                      options.Events.OnRedirectToAccessDenied = context =>
                      {
                          return Task.CompletedTask;
                      };
                  })
                .AddOpenIdConnect(options =>
                {
                    options.Authority = configuration["IdentityServer:Authority"];
                    options.RequireHttpsMetadata = true;
                    options.ClientId = configuration["Client:Id"];
                    //options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    //options.Backchannel = new IdentityServerClient(new )
                    options.BackchannelHttpHandler = new MtlsHandler(configuration, environment);
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                });

            return services;
        }
    }
}