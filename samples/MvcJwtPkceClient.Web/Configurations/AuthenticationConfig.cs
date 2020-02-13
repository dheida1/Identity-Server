using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Threading.Tasks;

namespace MvcJwtClient.Web.Configurations
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
                      // Expire the session of 15 minutes of inactivity
                      //this is diffferent than the access_token expiration
                      options.ExpireTimeSpan = TimeSpan.FromMinutes(15);

                      //as long as the user is actively using the MvcJwtClient application, 
                      //the session should remain active.
                      options.SlidingExpiration = true;
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
                    options.ResponseMode = "form_post"; //this allows us to keep codes out of the URL and protected via TLS
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("api1");
                    options.Scope.Add("api2");
                    options.Scope.Add("offline_access"); //need this to get back '.refreshToken' to use when calling api's   

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = JwtClaimTypes.Name,
                        RoleClaimType = JwtClaimTypes.Role
                    };
                    options.Events.OnAuthorizationCodeReceived = context =>
                    {
                        context.TokenEndpointRequest.ClientAssertionType = OidcConstants.ClientAssertionTypes.JwtBearer;
                        //context.TokenEndpointRequest.ClientAssertion = TokenGenerator.CreateClientAuthJwt(configuration["Client:Id"], );
                        context.TokenEndpointRequest.ClientAssertion = TokenGenerator.CreateClientAuthJwt();

                        return Task.CompletedTask;
                    };
                });
            return services;
        }
    }
}