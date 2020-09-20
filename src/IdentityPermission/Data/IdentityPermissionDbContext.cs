using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.ExtensionStore.IdentityPermission
{
    public class IdentityPermissionDbContext
        : IdentityPermissionDbContext<IdentityUser, IdentityRole, IdentityPermission, string>
    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityPermissionDbContext() { }
    }

    public class IdentityPermissionDbContext<TUser>
        : IdentityPermissionDbContext<TUser, IdentityRole<string>, IdentityPermission<string>, string>
            where TUser : IdentityUser
    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityPermissionDbContext() { }
    }
    public class IdentityPermissionDbContext<TUser, TRole>
        : IdentityPermissionDbContext<TUser, TRole, IdentityPermission<string>, string>
            where TUser : IdentityUser
            where TRole : IdentityRole
    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityPermissionDbContext() { }
    }

    public class IdentityPermissionDbContext<TUser, TRole, TPermission>
        : IdentityPermissionDbContext<IdentityUser<string>, IdentityRole<string>, IdentityPermission<string>, string>
            where TUser : IdentityUser
            where TRole : IdentityRole
            where TPermission : IdentityPermission
    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityPermissionDbContext() { }
    }

    public class IdentityPermissionDbContext<TUser, TRole, TPermission, TKey>
        : IdentityPermissionDbContext<IdentityUser<TKey>, IdentityRole<TKey>, IdentityPermission<TKey>, IdentityRolePermission<TKey>, TKey, IdentityUserRole<TKey>, IdentityRoleClaim<TKey>>
            where TPermission : IdentityPermission<TKey>
            where TKey : IEquatable<TKey>
    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }

        protected IdentityPermissionDbContext() { }
    }

    public class IdentityPermissionDbContext<TUser, TRole, TPermission, TRolePermission, TKey, TUserRole, TRoleClaim>
        : IdentityPermissionDbContext<IdentityUser<TKey>, IdentityRole<TKey>, TKey, IdentityUserClaim<TKey>,
            IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>,
            TPermission, TRolePermission>

      where TRole : IdentityRole<TKey>
      where TPermission : IdentityPermission<TKey>
      where TRolePermission : IdentityRolePermission<TKey>
      where TKey : IEquatable<TKey>
      where TUserRole : IdentityUserRole<TKey>
      where TRoleClaim : IdentityRoleClaim<TKey>

    {
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }
        protected IdentityPermissionDbContext() { }
    }

    public class IdentityPermissionDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken, TPermission, TRolePermission>
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
        public IdentityPermissionDbContext(DbContextOptions options) : base(options) { }
        protected IdentityPermissionDbContext() { }

        public virtual DbSet<TPermission> Permissions { get; set; }
        public virtual DbSet<TRolePermission> RolesPermissions { get; set; }

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