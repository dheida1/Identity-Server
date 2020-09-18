using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.ExtensionStore.IdentityPermission
{
    /// <summary>
    /// Provides an abstraction for a storage and management of permissions.
    /// </summary>
    /// <typeparam name="TPermission">The type that represents a permission.</typeparam>
    public interface IPermissionStore<TPermission> : IDisposable where TPermission : class
    {
        /// <summary>
        /// Creates a new permission in a store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission to create in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> CreateAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Updates a permission in a store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission to update in the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> UpdateAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Deletes a permission from the store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission to delete from the store.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous query.</returns>
        Task<IdentityResult> DeleteAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the ID for a permission from the store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the permission.</returns>
        Task<string> GetPermissionIdAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the name of a permission from the store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the permission.</returns>
        Task<string> GetPermissionNameAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Sets the name of a permission in the store as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission whose name should be set.</param>
        /// <param name="permissionName">The name of the permission.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetPermissionNameAsync(TPermission permission, string permissionName, CancellationToken cancellationToken);

        /// <summary>
        /// Get a permission's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission whose normalized name should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the permission.</returns>
        Task<string> GetNormalizedPermissionNameAsync(TPermission permission, CancellationToken cancellationToken);

        /// <summary>
        /// Set a permission's normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="permission">The permission whose normalized name should be set.</param>
        /// <param name="normalizedName">The normalized name to set</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetNormalizedPermissionNameAsync(TPermission permission, string normalizedName, CancellationToken cancellationToken);


        /// <summary>
        /// Finds the permission who has the specified ID as an asynchronous operation.
        /// </summary>
        /// <param name="permissionId">The permission ID to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        Task<TPermission> FindByIdAsync(string permissionId, CancellationToken cancellationToken);

        /// <summary>
        /// Finds the permission who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedpermissionName">The normalized permission name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        Task<TPermission> FindByNameAsync(string normalizedpermissionName, CancellationToken cancellationToken);
    }
}
