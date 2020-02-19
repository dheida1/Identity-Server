using Api2.Configurations;
using IdentityServer4.AccessTokenValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.Reflection;
using System.Security.Claims;
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
            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly);

            services.AddDataServices(Configuration);

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = IdentityServerAuthenticationDefaults.AuthenticationScheme;
            })
                 .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = "https://localhost:4300";
                     options.RequireHttpsMetadata = Environment.IsDevelopment() ? false : true;
                     options.ApiName = "api2";
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
                 });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("newpolicy", policy =>
                {
                    policy.RequireClaim("name");
                    policy.RequireClaim(claimType: ClaimTypes.Name);
                    policy.RequireClaim(claimType: ClaimTypes.Email);
                });
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
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
