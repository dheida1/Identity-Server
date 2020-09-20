using System.Linq;

namespace Identity.ExtensionStore.IdentityPermission
{
    // <summary>
    /// Provides an abstraction for querying permissions in a Permission store.
    /// </summary>
    /// <typeparam name="TPermission">The type encapsulating a permission.</typeparam>
    public interface IQueryablePermissionStore<TPermmission> : IPermissionStore<TPermmission> where TPermmission : class
    {
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of permissions.
        /// </summary>
        /// <value>An <see cref="IQueryable{T}"/> collection of permissions.</value>
        IQueryable<TPermmission> Permissions { get; }
    }
}
