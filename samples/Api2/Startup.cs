using Api2.Features.Authorize;
using Api2.Middlewares;
using IdentityModel;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Api2
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddAuthorization();

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationPolicyProvider, AuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, HasPermissionHandler>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            //services.AddDataServices(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                 .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = Configuration["IdentityServer:Address"];// https://localhost:4300";
                     options.RequireHttpsMetadata = Environment.IsDevelopment() ? false : true;
                     options.ApiName = "inventory";
                     options.SaveToken = true;
                     options.JwtBearerEvents.OnMessageReceived = context =>
                     {
                         return Task.FromResult(0);
                     };
                     options.JwtBearerEvents.OnAuthenticationFailed = context =>
                     {
                         return Task.FromResult(0);
                     };
                     options.JwtBearerEvents.OnTokenValidated = context =>
                     {
                         return Task.FromResult(0);
                     };
                 }).AddCertificate("x509", options =>
                 {
                     options.RevocationMode = (Environment.IsDevelopment() ? X509RevocationMode.NoCheck : X509RevocationMode.Online);
                     options.AllowedCertificateTypes = (Environment.IsDevelopment() ? CertificateTypes.SelfSigned : CertificateTypes.Chained);
                     options.ValidateCertificateUse = (Environment.IsDevelopment() ? false : true);
                     options.ValidateValidityPeriod = (Environment.IsDevelopment() ? false : true);

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
                 });


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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseCors(policy =>
            //{
            //    policy.WithOrigins(
            //        "https://localhost:4300",
            //        "https://localhost:5001");

            //    policy.AllowAnyHeader();
            //    policy.AllowAnyMethod();
            //    policy.WithExposedHeaders("WWW-Authenticate");
            //});

            app.UseHttpsRedirection();
            //mtls
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCertificateForwarding();

            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<ConfirmationValidationMiddleware>(new ConfirmationValidationMiddlewareOptions
            {
                CertificateSchemeName = "x509",
                JwtBearerSchemeName = IdentityServerAuthenticationDefaults.AuthenticationScheme
            });


            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
