using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.ExtensionStore.IdentityPermission
{

    public interface IRolePermissionStore<TPermission, TRole, TUser> : IPermissionStore<TRole>
        where TPermission : class
        where TRole : class
        where TUser : class
    {
        Task<IList<TPermission>> GetUserPermissions(TUser user, CancellationToken cancellationToken = default(CancellationToken));
        Task<IList<TPermission>> GetPermissionsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken));

        Task<IList<TRole>> GetRolesAsync(TPermission permission, CancellationToken cancellationToken = default(CancellationToken));

        Task AddRolePermissionAsync(TRole role, TPermission permission, CancellationToken cancellationToken = default(CancellationToken));

        Task RemoveRolePermissionAsync(TRole role, TPermission permission, CancellationToken cancellationToken = default(CancellationToken));
    }
}