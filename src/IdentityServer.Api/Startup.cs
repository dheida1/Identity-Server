using IdentityServer.Api.Configurations;
using IdentityServer.Api.IdentityServer.Api;
using IdentityServer.Infrastructure.Data;
using IdentityServer.Infrastructure.Entities;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using OtsLogger;
using System;
using System.Linq;

namespace IdentityServer.Api
{
    public class Startup
    {
        public IHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }
        public Startup(IHostEnvironment environment, IConfiguration configuration)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //TODO: Add DistributedCache
            //TODO: switch this to api with no views or possibly ng views

            var mvcBuilder = services.AddControllersWithViews();

            if (Environment.IsDevelopment())
            {
                mvcBuilder.AddRazorRuntimeCompilation();
            }

            services.AddIISConfigs()
                    .AddDataServices(Configuration)
                    .AddIdentityServerConfigs(Environment, Configuration)
                    .AddOtsAuthentication(Environment, Configuration);

            //custom ots logging tool
            services.AddOtsLogger(opt =>
            {
                //The unique IAM ("Uid") or ADFS ("http://la.gov/ObjectGUID") identifier. This is NOT the UserId.
                // The default value is "http://la.gov/ObjectGUID"
                opt.ObjectGuid = "http://la.gov/CVT-ObjectGUID";
            });

            //hsts
            if (!Environment.IsDevelopment())
            {
                services.AddHsts(options =>
                {
                    options.Preload = true;
                    options.IncludeSubDomains = true;
                    options.MaxAge = TimeSpan.FromDays(365);
                    options.ExcludedHosts.Add("identityserver.la.gov");
                });
            }

            //cors
            //if (!Environment.IsDevelopment())
            //{
            //    services.AddCors(options =>
            //    {
            //        options.AddPolicy("AllowAll",
            //            builder => builder
            //                .WithOrigins(
            //                    "https://localhost:5001",
            //                    "http://localhost:5002"
            //                    )
            //                .AllowAnyHeader()
            //                .AllowAnyMethod()
            //                .AllowCredentials());
            //    });

            //}
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // seeding
            InitializeDatabase(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                //****IMPORTANT SECURITY HEADERS******
                //test site status at https://securityheaders.com/
                //https://damienbod.com/2018/02/08/adding-http-headers-to-improve-security-in-an-asp-net-mvc-core-application/
                //https://www.c-sharpcorner.com/article/secure-web-application-using-http-security-headers-in-asp-net-core/
                //Registered before static files to always set header

                //Strict-Transport-Security Header
                //enforce that all communication is done over HTTPS. 
                //This will protect websites against SSL stripping, 
                //man-in-the-middle attacks by indicating to the browser to access 
                //the website using HTTPS instead of using HTTP and 
                //refuse to connect in case of certificate errors and warnings
                //it’s recommended to use HSTS on production only
                app.UseHsts();
            }

            //X-XSS-Protection Header
            //prevent cross-site scripting attack
            app.UseXXssProtection(options => options.EnabledWithBlockMode());

            //Content-Security-Policy Header
            //prevent code injection attacks like 
            //cross-site scripting and clickjacking 
            //or prevent mixed mode (HTTPS and HTTP)
            app.UseCsp(opts => opts
                .BlockAllMixedContent()
                .StyleSources(s => s.Self())
                .StyleSources(s => s.UnsafeInline())
                .FontSources(s => s.Self())
                .FormActions(s => s.Self())
                .FormActions(s => s.CustomSources("https://localhost/**"))
                .FormActions(s => s.CustomSources("*.la.gov"))
                .FormActions(s => s.CustomSources("https://localhost:5001/signin-oidc"))
                .FrameAncestors(s => s.Self())
                .ImageSources(s => s.Self())
                .ScriptSources(s => s.Self())
                .ScriptSources(s => s.CustomSources(Configuration["AppConfiguration:SecurityHeaders:CSP:ScriptSources"])) //this will change anytime js is modified
                .ReportUris(r => r.Uris("/home/report"))
        );

            app.UseCspReportOnly(options => options
                .DefaultSources(s => s.Self())
                .ImageSources(s => s.None()));

            //X-Frame-Options Header
            //disallow hackers from embedding content in an iframe
            //ensure that website content is not embedded into other 
            //sites and to prevent click jacking attacks
            app.UseXfo(options => options.Deny());

            //X-Content-Type-Options Header
            //used to disable the MIME-sniffing (where a hacker 
            //tries to exploit missing metadata on served files 
            //in browser) and can be set to no-sniff to prevent it
            app.UseXContentTypeOptions();

            //Referrer Policy Header
            //This header contains a site from which the user has been transferred.
            //But referrer URLs may contain sensitive data.If you don’t want to allow 
            //browsers to display your website as last visited in “Referer” header, 
            //we can use Referrer-Policy: no - referrer
            app.UseReferrerPolicy(opts => opts.NoReferrer());

            //Feature Policy Header
            //used to enable and disable certain web platform features like microphone, 
            //camera etc. on browser and those they embedded. 
            //This header is completely optional and based on the explicit requirement
            //app.Use(async (context, next) =>
            //{
            //    if (!context.Response.Headers.ContainsKey("Feature-Policy"))
            //    {
            //        context.Response.Headers.Add("Feature-Policy", "accelerometer 'none'; 
            //              camera 'none'; microphone 'none';");
            //    }
            //    await next();
            //});


            //for mtls
            app.UseCertificateForwarding();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseOtsLogger();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

        }

        private void InitializeDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();

                if (!context.Clients.Any())
                {
                    Console.WriteLine("Clients being populated");
                    foreach (var client in Config.GetClients())
                    {
                        context.Clients.Add(client.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Clients populated");
                }

                if (!context.IdentityResources.Any())
                {
                    Console.WriteLine("IdentityResources being populated");
                    foreach (var resource in Config.GetIdentityResources(Configuration))
                    {
                        context.IdentityResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("IdentityResources already populated");
                }

                if (!context.ApiResources.Any())
                {
                    Console.WriteLine("ApiResources being populated");
                    foreach (var resource in Config.GetApis(Configuration))
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("ApiResources already populated");
                }

                if (!context.ApiScopes.Any())
                {
                    Console.WriteLine("Scopes being populated");
                    foreach (var resource in Config.GetApiScopes(Configuration))
                    {
                        context.ApiScopes.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("IdentityResources already populated");
                }

                Console.WriteLine("Done seeding database.");
                Console.WriteLine();

                var aspIdentitycontext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                aspIdentitycontext.Database.Migrate();

                //permissions
                var permission1 = new ApplicationPermission
                {
                    Name = "OfficesOfficeCreate"
                };

                var permission2 = new ApplicationPermission
                {
                    Name = "OfficesOfficeContractInfoUpdate"
                };

                var permission3 = new ApplicationPermission
                {
                    Name = "OfficesOfficeHoursAndDaysInfoUpdate"
                };

                var permission4 = new ApplicationPermission
                {
                    Name = "InternalAnnouncementsDisplay"
                };
                var permission5 = new ApplicationPermission
                {
                    Name = "OfficesOfficeDisplay"
                };

                //roles
                var role1 = new ApplicationRole
                {
                    Name = "DPS-NEAL-Dev-OMV-Offices",
                    NormalizedName = "DPS-NEAL-DEV-OMV-OFFICES",
                };

                var role2 = new ApplicationRole
                {
                    Name = "DPS-NEAL-Dev-OMV-InternalAnnouncements",
                    NormalizedName = "DPS-NEAL-DEV-OMV-INTERNALANNOUNCEMENTS",
                };

                var role3 = new ApplicationRole
                {
                    Name = "DPS-NEAL-Dev-OMV-Offices-Regions",
                    NormalizedName = "DPS-NEAL-DEV-OMV-OFFICES-REGIONS",
                };

                if (!aspIdentitycontext.Permissions.Any())
                {
                    Console.WriteLine("Permissions being populated");

                    aspIdentitycontext.Permissions.Add(permission1);
                    aspIdentitycontext.Permissions.Add(permission2);
                    aspIdentitycontext.Permissions.Add(permission3);
                    aspIdentitycontext.Permissions.Add(permission4);
                }
                else
                {
                    Console.WriteLine("Permissions already populated");
                }


                if (!aspIdentitycontext.Roles.Any())
                {
                    Console.WriteLine("Roles being populated");

                    aspIdentitycontext.Roles.Add(role1);
                    aspIdentitycontext.Roles.Add(role2);
                    aspIdentitycontext.Roles.Add(role3);

                    aspIdentitycontext.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Roles already populated");
                }


                if (!aspIdentitycontext.RolePermissions.Any())
                {
                    Console.WriteLine("RolePermissions being populated");

                    aspIdentitycontext.RolePermissions.Add(
                        new ApplicationRolePermission
                        {
                            Role = role1,
                            Permission = permission1
                        });

                    aspIdentitycontext.RolePermissions.Add(
                       new ApplicationRolePermission
                       {
                           Role = role1,
                           Permission = permission2
                       });

                    aspIdentitycontext.RolePermissions.Add(
                      new ApplicationRolePermission
                      {
                          Role = role1,
                          Permission = permission3
                      });

                    aspIdentitycontext.RolePermissions.Add(
                    new ApplicationRolePermission
                    {
                        Role = role1,
                        Permission = permission4
                    });

                    aspIdentitycontext.RolePermissions.Add(
                     new ApplicationRolePermission
                     {
                         Role = role2,
                         Permission = permission4
                     });

                    aspIdentitycontext.RolePermissions.Add(
                     new ApplicationRolePermission
                     {
                         Role = role3,
                         Permission = permission4
                     });

                    aspIdentitycontext.SaveChanges();
                }

                Console.WriteLine("Done seeding database.");
                Console.WriteLine();
            }
        }
    }
}
