using System;

namespace Identity.ExtensionStore.IdentityPermission
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ExtensionStore.IdentityPermission.IdentityPermission{System.String}" />
    public class IdentityPermission : IdentityPermission<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityPermission"/> class.
        /// </summary>
        public IdentityPermission()
        {
            Id = Guid.NewGuid().ToString();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityPermission"/> class.
        /// </summary>
        /// <param name="permissionName">Name of the permission.</param>
        public IdentityPermission(string permissionName) : this()
        {
            Name = permissionName;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <seealso cref="ExtensionStore.IdentityPermission.IdentityPermission{System.String}" />
    public class IdentityPermission<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityPermission{TKey}" /> class.
        /// </summary>
        public IdentityPermission() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityPermission{TKey}"/> class.
        /// </summary>
        /// <param name="permissionName">Name of the permission.</param>
        public IdentityPermission(string permissionName) : this()
        {
            Name = permissionName;
        }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public virtual TKey Id { get; set; }
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public virtual string Name { get; set; }
        /// <summary>
        /// Gets or sets the name of the normalized.
        /// </summary>
        /// <value>
        /// The name of the normalized.
        /// </value>
        public virtual string NormalizedName { get; set; }
        /// <summary>
        /// Gets or sets the concurrency stamp.
        /// </summary>
        /// <value>
        /// The concurrency stamp.
        /// </value>
        public virtual string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
