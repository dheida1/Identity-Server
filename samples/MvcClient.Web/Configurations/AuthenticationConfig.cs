using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
                      options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
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
                    options.Authority = "https://localhost:4300";
                    options.RequireHttpsMetadata = true;
                    options.ClientId = "mvc";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                });

            return services;
        }
    }
}
