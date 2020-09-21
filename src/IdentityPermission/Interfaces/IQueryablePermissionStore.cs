using Identity.IdentityPermission;
using System.Linq;

namespace IdentityPermission.Interfaces
{
    public interface IQueryablePermissionStore<TPermission> : IPermissionStore<TPermission>
        where TPermission : class
    {
        IQueryable<TPermission> Permissions { get; }
    }
}
