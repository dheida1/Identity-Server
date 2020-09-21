using Microsoft.AspNetCore.Identity;

namespace Identity.IdentityPermission
{
    public class IdentityPermissionErrorDescriber
    {

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="permission"/> name is invalid.
        /// </summary>
        /// <param name="permission">The invalid permission.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specific permission <paramref name="permission"/> name is invalid.</returns>
        public virtual IdentityError InvalidPermissionName(string permission)
        {
            return new IdentityError
            {
                Code = nameof(InvalidPermissionName),
                Description = string.Format("Permission name '{0}' is invalid.", permission)
            };
        }

        /// <summary>
        /// Returns an <see cref="IdentityError"/> indicating the specified <paramref name="permission"/> name already exists.
        /// </summary>
        /// <param name="permission">The duplicate permission.</param>
        /// <returns>An <see cref="IdentityError"/> indicating the specific permission <paramref name="permission"/> name already exists.</returns>
        public virtual IdentityError DuplicatePermissionName(string permission)
        {
            return new IdentityError
            {
                Code = nameof(DuplicatePermissionName),
                Description = string.Format("Permission name '{0}' is already taken..", permission)
            };
        }
    }
}