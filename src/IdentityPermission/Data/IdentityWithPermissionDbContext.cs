using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.IdentityPermission
{
    public class IdentityWithPermissionDbContext
        : IdentityWithPermissionDbContext<IdentityUser, IdentityRole, IdentityPermission, string>
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityWithPermissionDbContext() { }
    }

    public class IdentityWithPermissionDbContext<TUser>
        : IdentityWithPermissionDbContext<TUser, IdentityRole<string>, IdentityPermission<string>, string>
            where TUser : IdentityUser
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityWithPermissionDbContext() { }
    }
    public class IdentityWithPermissionDbContext<TUser, TRole>
        : IdentityWithPermissionDbContext<TUser, TRole, IdentityPermission<string>, string>
            where TUser : IdentityUser
            where TRole : IdentityRole
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityWithPermissionDbContext() { }
    }

    public class IdentityWithPermissionDbContext<TUser, TRole, TPermission>
        : IdentityWithPermissionDbContext<IdentityUser<string>, IdentityRole<string>, IdentityPermission<string>, string>
            where TUser : IdentityUser
            where TRole : IdentityRole
            where TPermission : IdentityPermission
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityWithPermissionDbContext() { }
    }

    public class IdentityWithPermissionDbContext<TUser, TRole, TPermission, TKey>
        : IdentityWithPermissionDbContext<IdentityUser<TKey>, IdentityRole<TKey>, IdentityPermission<TKey>, IdentityRolePermission<TKey>, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>
            where TPermission : IdentityPermission<TKey>
            where TKey : IEquatable<TKey>
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityWithPermissionDbContext() { }
    }

    public class IdentityWithPermissionDbContext<TUser, TRole, TPermission, TRolePermission, TKey, TUserRole, TRoleClaim>
        : IdentityWithPermissionDbContext<IdentityUser<TKey>, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>,
            IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TPermission, TRolePermission>

      where TRole : IdentityRole<TKey>
      where TPermission : IdentityPermission<TKey>
      where TRolePermission : IdentityRolePermission<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : IdentityUserRole<TKey>
      where TRoleClaim : IdentityRoleClaim<TKey>

    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }
        protected IdentityWithPermissionDbContext() { }
    }

    public class IdentityWithPermissionDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TPermission, TRolePermission>
       : IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>

     where TUser : IdentityUser<TKey>
     where TRole : IdentityRole<TKey>
     where TPermission : IdentityPermission<TKey>
     where TRolePermission : IdentityRolePermission<TKey>
     where TKey : IEquatable<TKey>
     where TUserRole : IdentityUserRole<TKey>
     where TUserClaim : IdentityUserClaim<TKey>
     where TUserToken : IdentityUserToken<TKey>
     where TUserLogin : IdentityUserLogin<TKey>
     where TRoleClaim : IdentityRoleClaim<TKey>
    {
        public IdentityWithPermissionDbContext(DbContextOptions options) : base(options) { }
        protected IdentityWithPermissionDbContext() { }

        public virtual DbSet<TPermission> Permissions { get; set; }
        public virtual DbSet<TRolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TPermission>(b =>
            {
                b.HasKey(r => r.Id);
                b.HasIndex(r => r.NormalizedName).HasName("PermissionNameIndex").IsUnique();
                b.ToTable("AspNetPermissions");
                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

                b.Property(u => u.Name).HasMaxLength(256);
                b.Property(u => u.NormalizedName).HasMaxLength(256);

                b.HasMany<TRolePermission>().WithOne().HasForeignKey(ur => ur.PermissionId).IsRequired();
            });

            builder.Entity<TRole>(b =>
            {
                b.HasMany<TRolePermission>().WithOne().HasForeignKey(rp => rp.RoleId).IsRequired();
            });

            builder.Entity<TRolePermission>(b =>
            {
                b.HasKey(r => new { r.RoleId, r.PermissionId });
                b.ToTable("AspNetRolePermissions");
            });
        }
    }
}