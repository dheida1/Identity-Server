using IdentityServer.Infrastructure.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityServer.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        public DbSet<ApplicationPermission> Permissions { get; set; }
        public DbSet<ApplicationRolePermission> RolePermissions { get; set; }
        public DbSet<ApplicationProfile> Profiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //permissions with roles
            builder.Entity<ApplicationPermission>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("AspNetPermissions");
                b.Property(u => u.Name).HasMaxLength(256);
            });

            builder.Entity<ApplicationRolePermission>()
                .HasOne(e => e.Role)
                 .WithMany(rp => rp.RolePermissions)
                 .HasForeignKey(r => r.RoleId);

            builder.Entity<ApplicationRolePermission>()
               .HasOne(e => e.Permission)
               .WithMany(rp => rp.RolePermissions)
               .HasForeignKey(r => r.PermissionId);

            builder.Entity<ApplicationRolePermission>(b =>
            {
                b.HasKey(r => new { r.PermissionId, r.RoleId });
                b.ToTable("AspNetRolePermissions");
            });


            //profile with roles
            builder.Entity<ApplicationProfile>(b =>
            {
                b.HasKey(r => r.Id);
                b.ToTable("AspNetProfiles");
                b.Property(u => u.Name).HasMaxLength(256);
            });

            builder.Entity<ApplicationProfileRole>()
                .HasOne(e => e.Proflle)
                .WithMany(r => r.ProfileRoles)
                .HasForeignKey(r => r.ProfileId);

            builder.Entity<ApplicationProfileRole>()
               .HasOne(r => r.Role)
               .WithMany(r => r.ProfileRoles)
               .HasForeignKey(r => r.RoleId);

            builder.Entity<ApplicationProfileRole>(b =>
            {
                b.HasKey(r => new { r.ProfileId, r.RoleId });
                b.ToTable("ProfileRoles");
            });
        }
    }
}
