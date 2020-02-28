using IdentityServer.Infrastructure.Dto;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Infrastructure.Data
{
    public class ExtendedConfigurationDbContext : ConfigurationDbContext<ExtendedConfigurationDbContext>
    {
        public ExtendedConfigurationDbContext(DbContextOptions<ExtendedConfigurationDbContext> options,
            ConfigurationStoreOptions storeOptions
           ) : base(options, storeOptions) { }

        public DbSet<ExtClient> ExtClients { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ExtClient>(e =>
              {
                  e.HasOne<Client>();
              });
        }
    }
}