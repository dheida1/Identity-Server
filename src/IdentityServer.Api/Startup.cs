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

            //services.AddControllers();
            services.AddControllersWithViews();

            services.AddIISConfigs()
                    //.AddDatabase(Configuration)
                    .AddDataServices(Configuration)
                    .AddOtsAuthentication(Environment, Configuration)
                    .AddIdentityServerConfigs(Environment, Configuration);


            //services.AddCors(o => o.AddPolicy("ComeOnIn", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //}));
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //for mtls
            app.UseCertificateForwarding();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthentication();
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
                var permission1 = new Permission
                {
                    Name = "OfficesOfficeCreate",
                    TimeStamp = DateTime.Now
                };

                var permission2 = new Permission
                {
                    Name = "OfficesOfficeContractInfoUpdate",
                    TimeStamp = DateTime.Now
                };

                var permission3 = new Permission
                {
                    Name = "OfficesOfficeHoursAndDaysInfoUpdate",
                    TimeStamp = DateTime.Now
                };

                var permission4 = new Permission
                {
                    Name = "InternalAnnouncementsDisplay",
                    TimeStamp = DateTime.Now
                };
                var permission5 = new Permission
                {
                    Name = "OfficesOfficeDisplay",
                    TimeStamp = DateTime.Now
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
                        new RolePermission
                        {
                            Role = role1,
                            Permission = permission1
                        });

                    aspIdentitycontext.RolePermissions.Add(
                       new RolePermission
                       {
                           Role = role1,
                           Permission = permission2
                       });

                    aspIdentitycontext.RolePermissions.Add(
                      new RolePermission
                      {
                          Role = role1,
                          Permission = permission3
                      });

                    aspIdentitycontext.RolePermissions.Add(
                    new RolePermission
                    {
                        Role = role1,
                        Permission = permission4
                    });

                    aspIdentitycontext.RolePermissions.Add(
                     new RolePermission
                     {
                         Role = role2,
                         Permission = permission4
                     });

                    aspIdentitycontext.RolePermissions.Add(
                     new RolePermission
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
