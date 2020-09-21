using IdentityPermission.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.IdentityPermission
{
    public class RolePermissionStore<TRole, TPermission, TRolePermission, TKey>
       : RolePermissionStore<IdentityUser<TKey>, TRole, TPermission, TRolePermission, DbContext, TKey>
        where TRole : IdentityRole<TKey>, new()
        where TPermission : IdentityRole<TKey>, new()
        where TRolePermission : IdentityRolePermission<TKey>, new()
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="RoleStore{TRole}"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
        public RolePermissionStore(DbContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }
    }

    public class RolePermissionStore<TUser, TRole, TPermission, TRolePermission, TContext, TKey>
        : IRolePermissionStore<TRole, TPermission>
        //IQueryableRolePermissionStore<TPermission>
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TPermission : IdentityRole<TKey>
        where TContext : DbContext
        where TKey : IEquatable<TKey>
    {
        public RolePermissionStore(TContext context,
            // IQueryablePermissionStore<TPermission> permissionStore,
            // IQueryableRoleStore<TRole> roleStore,
            IdentityErrorDescriber describer = null)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            Context = context;
            ErrorDescriber = describer ?? new IdentityErrorDescriber();
            //PermissionStore = permissionStore;
            //RoleStore = roleStore;
        }

        public IdentityErrorDescriber ErrorDescriber { get; set; }
        public virtual TContext Context { get; private set; }
        private DbSet<TRolePermission> RolePermissions { get { return Context.Set<TRolePermission>(); } }
        public IQueryablePermissionStore<TPermission> PermissionStore { get; private set; }
        public IQueryableRoleStore<TRole> RoleStore { get; private set; }

        private bool _disposed;

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the stores
        /// </summary>
        public void Dispose() => _disposed = true;

        public Task AddToRoleAsync(TPermission permission, string roleName, CancellationToken cancellationToken)
        {
            var role = RoleStore.FindByNameAsync(roleName, cancellationToken);

            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(TPermission permission, string rolenName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(TPermission permission, CancellationToken cancellationToken)
        {
            var roles = await Permissions.Include(p => p.Find()
        }

        public Task<bool> IsInRoleAsync(TPermission permission, string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IList<TPermission>> GetRoleInPermissionsAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
