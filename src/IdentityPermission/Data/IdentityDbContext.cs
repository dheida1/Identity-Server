//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore;
//using System;

//namespace Identity.ExtensionStore.IdentityPermission

//{
//    ///// <summary>
//    ///// Base class for the Entity Framework database context used for identity.
//    ///// </summary>
//    //public class IdentityExtensionDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
//    //{
//    //    /// <summary>
//    //    /// Initializes a new instance of <see cref="IdentityDbContext"/>.
//    //    /// </summary>
//    //    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
//    //    public IdentityExtensionDbContext(DbContextOptions options) : base(options) { }

//    //    /// <summary>
//    //    /// Initializes a new instance of the <see cref="IdentityDbContext" /> class.
//    //    /// </summary>
//    //    protected IdentityExtensionDbContext() { }
//    //}

//    ///// <summary>
//    ///// Base class for the Entity Framework database context used for identity.
//    ///// </summary>
//    ///// <typeparam name="TUser">The type of the user objects.</typeparam>
//    //public class IdentityExtensionDbContext<TUser> : IdentityDbContext<TUser, IdentityRole, string> where TUser : IdentityUser
//    //{
//    //    /// <summary>
//    //    /// Initializes a new instance of <see cref="IdentityDbContext"/>.
//    //    /// </summary>
//    //    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
//    //    public IdentityExtensionDbContext(DbContextOptions options) : base(options) { }

//    //    /// <summary>
//    //    /// Initializes a new instance of the <see cref="IdentityDbContext" /> class.
//    //    /// </summary>
//    //    protected IdentityExtensionDbContext() { }
//    //}

//    ///// <summary>
//    ///// Base class for the Entity Framework database context used for identity.
//    ///// </summary>
//    ///// <typeparam name="TUser">The type of user objects.</typeparam>
//    ///// <typeparam name="TRole">The type of role objects.</typeparam>
//    ///// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
//    //public class IdentityExtensionDbContext<TUser, TRole, TPermission, TKey> : IdentityExtensionDbContext<TUser, TRole, TPermission, IdentityRolePermission<TKey>, TKey>
//    //    //IdentityDbContext<TUser, TRole, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
//    //    where TUser : IdentityUser<TKey>
//    //    where TRole : IdentityRole<TKey>
//    //    where TPermission : IdentityPermission<TKey>
//    //    where TKey : IEquatable<TKey>
//    //{
//    //    /// <summary>
//    //    /// Initializes a new instance of the db context.
//    //    /// </summary>
//    //    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
//    //    public IdentityExtensionDbContext(DbContextOptions options) : base(options) { }
//    //    /// <summary>
//    //    /// Initializes a new instance of the class.
//    //    /// </summary>
//    //    protected IdentityExtensionDbContext() { }
//    //}

//    //public class IdentityExtensionDbContext<TUser, TRole, TPermission, TRolePermission, TKey> : IdentityExtensionDbContext<TUser, TRole, TPermission, TRolePermission, TKey, IdentityUserClaim<TKey>, IdentityUserRole<TKey>, IdentityUserLogin<TKey>, IdentityRoleClaim<TKey>, IdentityUserToken<TKey>>
//    //    where TUser : IdentityUser<TKey>
//    //    where TRole : IdentityRole<TKey>
//    //    where TPermission : IdentityPermission<TKey>
//    //    where TRolePermission : IdentityRolePermission<TKey>
//    //    where TKey : IEquatable<TKey>
//    //{
//    //    /// <summary>
//    //    /// Initializes a new instance of the db context.
//    //    /// </summary>
//    //    /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
//    //    public IdentityExtensionDbContext(DbContextOptions options) : base(options) { }

//    //    /// <summary>
//    //    /// Initializes a new instance of the class.
//    //    /// </summary>
//    //    protected IdentityExtensionDbContext() { }
//    //}


//    public abstract class IdentityExtensionDbContext<TUser, TRole, TPermission, TRolePermission, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken> :
//       IdentityDbContext<TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken>

//       where TUser : IdentityUser<TKey>
//       where TRole : IdentityRole<TKey>
//       where TPermission : IdentityPermission<TKey>
//       where TRolePermission : IdentityRolePermission<TKey>
//       where TKey : IEquatable<TKey>
//       where TUserClaim : IdentityUserClaim<TKey>
//       where TUserRole : IdentityUserRole<TKey>
//       where TUserLogin : IdentityUserLogin<TKey>
//       where TRoleClaim : IdentityRoleClaim<TKey>
//       where TUserToken : IdentityUserToken<TKey>

//    {
//        /// <summary>
//        /// Initializes a new instance of the class.
//        /// </summary>
//        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
//        public IdentityExtensionDbContext(DbContextOptions options) : base(options) { }

//        /// <summary>
//        /// Initializes a new instance of the class.
//        /// </summary>
//        protected IdentityExtensionDbContext() { }

//        /// <summary>
//        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User roles.
//        /// </summary>
//       // public virtual DbSet<TUserRole> UserRoles { get; set; }

//        /// <summary>
//        /// Gets or sets the <see cref="DbSet{TEntity}"/> of roles.
//        /// </summary>
//        //public virtual DbSet<TRole> Roles { get; set; }

//        public virtual DbSet<TPermission> Permissions { get; set; }
//        public virtual DbSet<TRolePermission> RolesPermissions { get; set; }

//        /// <summary>
//        /// Gets or sets the <see cref="DbSet{TEntity}"/> of role claims.
//        /// </summary>
//        //public virtual DbSet<TRoleClaim> RoleClaims { get; set; }

//        /// <summary>
//        /// Configures the schema needed for the identity framework.
//        /// </summary>
//        /// <param name="builder">
//        /// The builder being used to construct the model for this context.
//        /// </param>
//        protected override void OnModelCreating(ModelBuilder builder)
//        {
//            base.OnModelCreating(builder);

//            builder.Entity<TPermission>(b =>
//            {
//                b.HasKey(r => r.Id);
//                b.HasIndex(r => r.NormalizedName).HasName("PermissionNameIndex").IsUnique();
//                b.ToTable("AspNetPermissions");
//                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

//                b.Property(u => u.Name).HasMaxLength(256);
//                b.Property(u => u.NormalizedName).HasMaxLength(256);

//                b.HasMany<TRolePermission>().WithOne().HasForeignKey(ur => ur.PermissionId).IsRequired();
//            });

//            builder.Entity<TRole>(b =>
//            {
//                b.HasKey(r => r.Id);
//                b.HasIndex(r => r.NormalizedName).HasName("RoleNameIndex").IsUnique();
//                b.ToTable("AspNetRoles");
//                b.Property(r => r.ConcurrencyStamp).IsConcurrencyToken();

//                b.Property(u => u.Name).HasMaxLength(256);
//                b.Property(u => u.NormalizedName).HasMaxLength(256);

//                b.HasMany<TUserRole>().WithOne().HasForeignKey(ur => ur.RoleId).IsRequired();
//                b.HasMany<TRoleClaim>().WithOne().HasForeignKey(rc => rc.RoleId).IsRequired();
//                b.HasMany<TRolePermission>().WithOne().HasForeignKey(rp => rp.RoleId).IsRequired();
//            });

//            builder.Entity<TRolePermission>(b =>
//            {
//                b.HasKey(r => new { r.RoleId, r.PermissionId });
//                b.ToTable("AspNetRolesPermissions");
//            });
//        }
//    }

//}
