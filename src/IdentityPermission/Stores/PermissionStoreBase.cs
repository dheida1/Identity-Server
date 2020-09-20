
//using Microsoft.AspNetCore.Identity;
//using System;
//using System.ComponentModel;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Identity.ExtensionStore.IdentityPermission
//{
//    /// <summary>
//    /// Creates a new instance of a persistence store for roles.
//    /// </summary>
//    /// <typeparam name="TRole">The type of the class representing a role.</typeparam>
//    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
//    /// <typeparam name="TUserRole">The type of the class representing a user role.</typeparam>
//    /// <typeparam name="TRoleClaim">The type of the class representing a role claim.</typeparam>
//    public abstract class PermissionStoreBase<TPermission, TKey, TRolePermission> : IQueryablePermissionStore<TPermission>
//        where TPermission : IdentityPermission<TKey>
//        where TKey : IEquatable<TKey>
//        where TRolePermission : IdentityRolePermission<TKey>, new()
//    {
//        /// <summary>
//        /// Constructs a new instance of <see cref="RoleStoreBase{TRole, TKey, TUserRole, TRoleClaim}"/>.
//        /// </summary>
//        /// <param name="describer">The <see cref="IdentityErrorDescriber"/>.</param>
//        public PermissionStoreBase(IdentityErrorDescriber describer)
//        {
//            if (describer == null)
//            {
//                throw new ArgumentNullException(nameof(describer));
//            }

//            ErrorDescriber = describer;
//        }

//        private bool _disposed;

//        /// <summary>
//        /// Gets or sets the <see cref="IdentityErrorDescriber"/> for any error that occurred with the current operation.
//        /// </summary>
//        public IdentityErrorDescriber ErrorDescriber { get; set; }

//        /// <summary>
//        /// Creates a new role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to create in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public abstract Task<IdentityResult> CreateAsync(TPermission role, CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// Updates a role in a store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to update in the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public abstract Task<IdentityResult> UpdateAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// Deletes a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role to delete from the store.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
//        public abstract Task<IdentityResult> DeleteAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// Gets the ID for a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose ID should be returned.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
//        public virtual Task<string> GetPermissionIdAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (permission == null)
//            {
//                throw new ArgumentNullException(nameof(permission));
//            }
//            return Task.FromResult(ConvertIdToString(permission.Id));
//        }

//        /// <summary>
//        /// Gets the name of a role from the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose name should be returned.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
//        public virtual Task<string> GetPermissionNameAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (permission == null)
//            {
//                throw new ArgumentNullException(nameof(permission));
//            }
//            return Task.FromResult(permission.Name);
//        }

//        /// <summary>
//        /// Sets the name of a role in the store as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose name should be set.</param>
//        /// <param name="roleName">The name of the role.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public virtual Task SetPermissionNameAsync(TPermission permission, string permissionName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (permission == null)
//            {
//                throw new ArgumentNullException(nameof(permission));
//            }
//            permission.Name = permissionName;
//            return Task.CompletedTask;
//        }

//        /// <summary>
//        /// Converts the provided <paramref name="id"/> to a strongly typed key object.
//        /// </summary>
//        /// <param name="id">The id to convert.</param>
//        /// <returns>An instance of <typeparamref name="TKey"/> representing the provided <paramref name="id"/>.</returns>
//        public virtual TKey ConvertIdFromString(string id)
//        {
//            if (id == null)
//            {
//                return default(TKey);
//            }
//            return (TKey)TypeDescriptor.GetConverter(typeof(TKey)).ConvertFromInvariantString(id);
//        }

//        /// <summary>
//        /// Converts the provided <paramref name="id"/> to its string representation.
//        /// </summary>
//        /// <param name="id">The id to convert.</param>
//        /// <returns>An <see cref="string"/> representation of the provided <paramref name="id"/>.</returns>
//        public virtual string ConvertIdToString(TKey id)
//        {
//            if (id.Equals(default(TKey)))
//            {
//                return null;
//            }
//            return id.ToString();
//        }

//        /// <summary>
//        /// Finds the role who has the specified ID as an asynchronous operation.
//        /// </summary>
//        /// <param name="id">The role ID to look for.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
//        public abstract Task<TPermission> FindByIdAsync(string id, CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// Finds the role who has the specified normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="normalizedName">The normalized role name to look for.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
//        public abstract Task<TPermission> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default(CancellationToken));

//        /// <summary>
//        /// Get a role's normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose normalized name should be retrieved.</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
//        public Task<string> GetNormalizedPermissionNameAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (permission == null)
//            {
//                throw new ArgumentNullException(nameof(permission));
//            }
//            return Task.FromResult(permission.NormalizedName);
//        }

//        /// <summary>
//        /// Set a role's normalized name as an asynchronous operation.
//        /// </summary>
//        /// <param name="role">The role whose normalized name should be set.</param>
//        /// <param name="normalizedName">The normalized name to set</param>
//        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
//        public virtual Task SetNormalizedPermissionNameAsync(TPermission permission, string normalizedName, CancellationToken cancellationToken = default(CancellationToken))
//        {
//            cancellationToken.ThrowIfCancellationRequested();
//            ThrowIfDisposed();
//            if (permission == null)
//            {
//                throw new ArgumentNullException(nameof(permission));
//            }
//            permission.NormalizedName = normalizedName;
//            return Task.CompletedTask;
//        }

//        /// <summary>
//        /// Throws if this class has been disposed.
//        /// </summary>
//        protected void ThrowIfDisposed()
//        {
//            if (_disposed)
//            {
//                throw new ObjectDisposedException(GetType().Name);
//            }
//        }

//        /// <summary>
//        /// Dispose the stores
//        /// </summary>
//        public void Dispose() => _disposed = true;

//        /// <summary>
//        /// A navigation property for the roles the store contains.
//        /// </summary>
//        public IQueryable<TPermission> Permissions
//        {
//            get;
//        }
//    }
//}