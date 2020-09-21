using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.IdentityPermission
{
    /// <summary>
    /// Provides the APIs for managing permissions in a persistence store.
    /// </summary>
    /// <typeparam name="TPermission">The type encapsulating a permission.</typeparam>
    public class PermissionManager<TPermission> : IDisposable where TPermission : class
    {
        private bool _disposed;

        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Constructs a new instance of <see cref="PermissionManager{TPermission}"/>.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="permissionValidators">A collection of validators for permissions.</param>
        /// <param name="keyNormalizer">The normalizer to use when normalizing permission names to keys.</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        public PermissionManager(
            IPermissionStore<TPermission> store,
            IEnumerable<IPermissionValidator<TPermission>> permissionValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<PermissionManager<TPermission>> logger)
        {
            if (store == null)
            {
                throw new ArgumentNullException(nameof(store));
            }
            Store = store;
            KeyNormalizer = keyNormalizer;
            ErrorDescriber = errors;
            Logger = logger;

            if (permissionValidators != null)
            {
                foreach (var v in permissionValidators)
                {
                    PermissionValidators.Add(v);
                }
            }
        }

        /// <summary>
        /// Gets the persistence store this instance operates over.
        /// </summary>
        /// <value>The persistence store this instance operates over.</value>
        protected IPermissionStore<TPermission> Store { get; private set; }

        /// <summary>
        /// Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// Gets a list of validators for permissions to call before persistence.
        /// </summary>
        /// <value>A list of validators for permissions to call before persistence.</value>
        public IList<IPermissionValidator<TPermission>> PermissionValidators { get; } = new List<IPermissionValidator<TPermission>>();

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages.
        /// </summary>
        /// <value>
        /// The <see cref="IdentityErrorDescriber"/> used to provider error messages.
        /// </value>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// Gets the normalizer to use when normalizing permission names to keys.
        /// </summary>
        /// <value>
        /// The normalizer to use when normalizing permission names to keys.
        /// </value>
        public ILookupNormalizer KeyNormalizer { get; set; }

        /// <summary>
        /// Gets an IQueryable collection of Permissions if the persistence store is an <see cref="IQueryablePermissionStore{TPermission}"/>,
        /// otherwise throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <value>An IQueryable collection of Permissions if the persistence store is an <see cref="IQueryablePermissionStore{TPermission}"/>.</value>
        /// <exception cref="NotSupportedException">Thrown if the persistence store is not an <see cref="IQueryablePermissionStore{TPermission}"/>.</exception>
        /// <remarks>
        /// Callers to this property should use <see cref="SupportsQueryablePermissions"/> to ensure the backing permission store supports 
        /// returning an IQueryable list of permissions.
        /// </remarks>
        //public virtual IQueryable<TPermission> Permissions
        //{
        //    get
        //    {
        //        var queryableStore = Store as IQueryablePermissionStore<TPermission>;
        //        if (queryableStore == null)
        //        {
        //            throw new NotSupportedException("Store does not implement IQueryableRoleStore<TPermission>.");
        //        }
        //        return queryableStore.Permissions;
        //    }
        //}

        /// <summary>
        /// Gets a flag indicating whether the underlying persistence store supports returning an <see cref="IQueryable"/> collection of permissions.
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports returning an <see cref="IQueryable"/> collection of permissions, otherwise false.
        /// </value>
        //public virtual bool SupportsQueryablePermissions
        //{
        //    get
        //    {
        //        ThrowIfDisposed();
        //        return Store is IQueryablePermissionStore<TPermission>;
        //    }
        //}

        /// <summary>
        /// Creates the specified <paramref name="permission"/> in the persistence store.
        /// </summary>
        /// <param name="permission">The permission to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task<IdentityResult> CreateAsync(TPermission permission)
        {
            ThrowIfDisposed();
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            var result = await ValidatePermissionAsync(permission);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedPermissionNameAsync(permission);
            result = await Store.CreateAsync(permission, CancellationToken);
            return result;
        }

        /// <summary>
        /// Updates the normalized name for the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission whose normalized name needs to be updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateNormalizedPermissionNameAsync(TPermission permission)
        {
            var name = await GetPermissionNameAsync(permission);
            await Store.SetNormalizedPermissionNameAsync(permission, NormalizeKey(name), CancellationToken);
        }

        /// <summary>
        /// Updates the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission to updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.
        /// </returns>
        public virtual Task<IdentityResult> UpdateAsync(TPermission permission)
        {
            ThrowIfDisposed();
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }

            return UpdatePermissionAsync(permission);
        }

        /// <summary>
        /// Deletes the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.
        /// </returns>
        public virtual Task<IdentityResult> DeleteAsync(TPermission permission)
        {
            ThrowIfDisposed();
            if (permission == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }

            return Store.DeleteAsync(permission, CancellationToken);
        }

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="permissionName"/> exists.
        /// </summary>
        /// <param name="permissionName">The permission name whose existence should be checked.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing true if the permission name exists, otherwise false.
        /// </returns>
        public virtual async Task<bool> PermissionExistsAsync(string permissionName)
        {
            ThrowIfDisposed();
            if (permissionName == null)
            {
                throw new ArgumentNullException(nameof(permissionName));
            }

            return await FindByNameAsync(NormalizeKey(permissionName)) != null;
        }

        /// <summary>
        /// Gets a normalized representation of the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The value to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key"/>.</returns>
        public virtual string NormalizeKey(string key)
        {
            return (KeyNormalizer == null) ? key : KeyNormalizer.NormalizeName(key);
        }

        /// <summary>
        /// Finds the permission associated with the specified <paramref name="permissionId"/> if any.
        /// </summary>
        /// <param name="permissionId">The permission ID whose permission should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the permission 
        /// associated with the specified <paramref name="permissionId"/>
        /// </returns>
        public virtual Task<TPermission> FindByIdAsync(string permissionId)
        {
            ThrowIfDisposed();
            return Store.FindByIdAsync(permissionId, CancellationToken);
        }

        /// <summary>
        /// Gets the name of the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission whose name should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the name of the 
        /// specified <paramref name="permission"/>.
        /// </returns>
        public virtual Task<string> GetPermissionNameAsync(TPermission permission)
        {
            ThrowIfDisposed();
            return Store.GetPermissionNameAsync(permission, CancellationToken);
        }

        /// <summary>
        /// Sets the name of the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission whose name should be set.</param>
        /// <param name="name">The name to set.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> SetPermissionNameAsync(TPermission permission, string name)
        {
            ThrowIfDisposed();

            await Store.SetPermissionNameAsync(permission, name, CancellationToken);
            await UpdateNormalizedPermissionNameAsync(permission);
            return IdentityResult.Success;
        }

        /// <summary>
        /// Gets the ID of the specified <paramref name="permission"/>.
        /// </summary>
        /// <param name="permission">The permission whose ID should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the 
        /// specified <paramref name="permission"/>.
        /// </returns>
        public virtual Task<string> GetPermissionIdAsync(TPermission permission)
        {
            ThrowIfDisposed();
            return Store.GetPermissionIdAsync(permission, CancellationToken);
        }

        /// <summary>
        /// Finds the permission associated with the specified <paramref name="permissionName"/> if any.
        /// </summary>
        /// <param name="permissionName">The name of the permission to be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the permission 
        /// associated with the specified <paramref name="permissionName"/>
        /// </returns>
        public virtual Task<TPermission> FindByNameAsync(string permissionName)
        {
            ThrowIfDisposed();
            if (permissionName == null)
            {
                throw new ArgumentNullException(nameof(permissionName));
            }

            return Store.FindByNameAsync(NormalizeKey(permissionName), CancellationToken);
        }

        /// <summary>
        /// Releases all resources used by the permission manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the permission manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before saving the permission via Create or Update.
        /// </summary>
        /// <param name="permission">The permission</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<IdentityResult> ValidatePermissionAsync(TPermission permission)
        {
            var errors = new List<IdentityError>();
            foreach (var v in PermissionValidators)
            {
                var result = await v.ValidateAsync(this, permission);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(0, "Permission {permissionId} validation failed: {errors}.", await GetPermissionIdAsync(permission), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Called to update the permission after validating and updating the normalized permission name.
        /// </summary>
        /// <param name="permission">The permission.</param>
        /// <returns>Whether the operation was successful.</returns>
        protected virtual async Task<IdentityResult> UpdatePermissionAsync(TPermission permission)
        {
            var result = await ValidatePermissionAsync(permission);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedPermissionNameAsync(permission);
            return await Store.UpdateAsync(permission, CancellationToken);
        }

        public virtual async Task<IdentityResult> RemoveFromRolesAsync(TPermission permission, IEnumerable<string> roles)
        {
            ThrowIfDisposed();
            var rolePermissionStore = GetRolePermissionStore();
            if (rolePermissionStore == null)
            {
                throw new ArgumentNullException(nameof(permission));
            }
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            foreach (var role in roles)
            {
                var normalizedRole = NormalizeName(role);
                if (!await rolePermissionStore.IsInRoleAsync(permission, normalizedRole, CancellationToken))
                {
                    return await RoleNotInPermissionError(permission, role);
                }
                await rolePermissionStore.RemoveFromRoleAsync(permission, normalizedRole, CancellationToken);
            }
            return await UpdatePermissionAsync(permission);
        }

        private async Task<IdentityResult> RoleNotInPermissionError(TPermission permission, string role)
        {
            Logger.LogWarning(6, "Permission {0} is not in role {role}.", await GetPermissionNameAsync(permission), role);
            return IdentityResult.Failed(ErrorDescriber.UserNotInRole(role));
        }

        private IRolePermissionStore<TPermission> GetRolePermissionStore()
        {
            var cast = Store as IRolePermissionStore<TPermission>;
            if (cast == null)
            {
                throw new NotSupportedException(string.Format("Store does not implement IUserRoleStore<TUser>."));
            }
            return cast;
        }

        /// <summary>
        /// Normalize user or role name for consistent comparisons.
        /// </summary>
        /// <param name="name">The name to normalize.</param>
        /// <returns>A normalized value representing the specified <paramref name="name"/>.</returns>
        public virtual string NormalizeName(string name)
            => (KeyNormalizer == null) ? name : KeyNormalizer.NormalizeName(name);

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
    }
}