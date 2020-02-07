using IdentityModel;
using IdentityServer.Api.IdentityServer.Api;
using IdentityServer.Core.Entities;
using IdentityServer.Infrastructure.Data;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SamlCore.AspNetCore.Authentication.Saml2.Metadata;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace IdentityServer.Api
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
            //TODO: Add DistributedCache
            //TODO: switch this to api with no views or possibly ng views

            //services.AddControllers();
            services.AddControllersWithViews();
            services.AddTransient<IProfileService, ProfileService>();

            // configures IIS out-of-proc settings (see https://github.com/aspnet/AspNetCore/issues/14882)
            services.Configure<IISOptions>(iis =>
                {
                    iis.AuthenticationDisplayName = "Windows";
                    iis.AutomaticAuthentication = false;
                });

            // configures IIS in-proc settings
            services.Configure<IISServerOptions>(iis =>
            {
                iis.AuthenticationDisplayName = "Windows";
                iis.AutomaticAuthentication = false;
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

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var migrationsAssembly = "IdentityServer.Infrastructure";

            services.AddDbContext<ApplicationDbContext>(builder =>
             builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication()
            .AddCertificate("x509", options =>
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
            })
            .AddSamlCore("adfs", options =>
              {
                  options.SignInScheme = IdentityConstants.ExternalScheme;
                  options.ServiceProvider.EntityId = Configuration["AppConfiguration:ServiceProvider:EntityId"];
                  options.MetadataAddress = Configuration["AppConfiguration:IdentityProvider:MetadataAddress"];

                  options.RequireMessageSigned = false;
                  options.WantAssertionsSigned = true;
                  options.CreateMetadataFile = true;

                  if (Environment.IsDevelopment())
                  {
                      options.ServiceProvider.X509Certificate2 = new X509Certificate2(@"Certificates/IdentityServer.pfx",
                         "1234", X509KeyStorageFlags.Exportable);
                  }
                  else
                  {
                      //if you want to search in cert store - can be used for production
                      options.ServiceProvider.X509Certificate2 = new Cryptography.X509Certificates.Extension.X509Certificate2(
                          Configuration["AppConfiguration:ServiceProvider:Certificate"],
                          StoreName.My,
                          StoreLocation.LocalMachine,
                          X509FindType.FindBySerialNumber);
                  }
                  options.ForceAuthn = true;
                  options.WantAssertionsSigned = true;
                  options.RequireMessageSigned = false;

                  options.ServiceProvider.ServiceName = "My Test Site";
                  options.ServiceProvider.Language = "en-US";
                  options.ServiceProvider.OrganizationDisplayName = "Louisiana State Government";
                  options.ServiceProvider.OrganizationName = "Louisiana State Government";
                  options.ServiceProvider.OrganizationURL = "https://ots.la.gov";
                  options.ServiceProvider.ContactPerson = new ContactType()
                  {
                      Company = "Louisiana State Government - OTS",
                      GivenName = "Dina Heidar",
                      EmailAddress = new[] { "dina.heidar@la.gov" },
                      contactType = ContactTypeType.technical,
                      TelephoneNumber = new[] { "+1 234 5678" }
                  };
                  options.Events.OnRemoteFailure = context =>
                  {
                      return Task.FromResult(0);
                  };
                  options.Events.OnTicketReceived = context =>
                  {
                      var identity = context.Principal.Identity as ClaimsIdentity;
                      var claims = context.Principal.Claims;
                      var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;
                      identity.AddClaim(new Claim(ClaimTypes.Email, name));
                      return Task.FromResult(0);
                  };
              });

            var identityServer = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                options.MutualTls.Enabled = true;
                options.MutualTls.ClientCertificateAuthenticationScheme = "x509";
            })
                 .AddAspNetIdentity<ApplicationUser>()
                 .AddProfileService<ProfileService>()
                 .AddJwtBearerClientAuthentication() //to accept clients via jwts

                 // this adds the config data from DB (clients, resources, CORS)
                 .AddConfigurationStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                             builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                             sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));
                 })
                 // this adds the operational data from DB (codes, tokens, consents)
                 .AddOperationalStore(options =>
                 {
                     options.ConfigureDbContext = builder =>
                              builder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                              sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly));

                     // this enables automatic token cleanup. this is optional.
                     options.EnableTokenCleanup = true;
                 });

            identityServer.AddMutualTlsSecretValidators();

            //services.AddCors(o => o.AddPolicy("ComeOnIn", builder =>
            //{
            //    builder.AllowAnyOrigin()
            //           .AllowAnyMethod()
            //           .AllowAnyHeader();
            //}));

            if (Environment.IsDevelopment())
            {
                identityServer.AddDeveloperSigningCredential();
            }
            else
            {
                //identityServer.AddSigningCredential();
                throw new Exception("need to configure key material");
            }
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

            app.UseCertificateForwarding();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseIdentityServer();
            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
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
                    foreach (var resource in Config.GetIdentityResources())
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
                    foreach (var resource in Config.GetApis())
                    {
                        context.ApiResources.Add(resource.ToEntity());
                    }
                    context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("ApiResources already populated");
                }

                Console.WriteLine("Done seeding database.");
                Console.WriteLine();

                serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
            }
        }
    }
}
