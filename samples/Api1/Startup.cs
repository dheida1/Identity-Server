using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using System.Threading.Tasks;

namespace Api1
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

            services.AddAuthentication()
                 .AddIdentityServerAuthentication(options =>
                 {
                     options.Authority = "https://localhost:4300";
                     options.RequireHttpsMetadata = true;
                     options.ApiName = "test_api";
                     options.ApiSecret = "secret";
                     //options.RoleClaimType = "role";
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
            //        "https://localhost:5002",
            //        "https://localhost:5000");

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
