using System;

namespace Identity.IdentityPermission
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    public class IdentityRolePermission<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the permissionthat is linked to a role.
        /// </summary>
        /// <value>
        /// The role identifier.
        /// </value>

        public virtual TKey RoleId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the permission that is linked to the role.
        /// </summary>
        /// <value>
        /// The permission identifier.
        public virtual TKey PermissionId { get; set; }
    }
}
