using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Interfaces
{
    public interface IUserPermissionStore<TUser>
        where TUser : class
    {
        Task<List<string>> GetUserPermissions(TUser user, CancellationToken cancellationToken = default(CancellationToken));
    }
}
