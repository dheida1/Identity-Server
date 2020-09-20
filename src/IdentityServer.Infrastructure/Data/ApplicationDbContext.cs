using Identity.ExtensionStore.IdentityPermission;
using IdentityServer.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer.Infrastructure.Data
{
    public class ApplicationDbContext
        : IdentityPermissionDbContext<ApplicationUser, ApplicationRole, ApplicationPermission, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
