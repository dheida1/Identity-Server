using IdentityServer.Infrastructure.Data;
using IdentityServer.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace IdentityServer.Infrastructure.Stores
{
    public class UserPermissionStore<TUser> : IUserPermissionStore<TUser>
        where TUser : class
    {
        private ApplicationDbContext _context;
        private UserManager<TUser> _userManager;
        public UserPermissionStore(
            ApplicationDbContext context,
            UserManager<TUser> userManager
            )
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<List<string>> GetUserPermissions(TUser user, CancellationToken cancellationToken = default(CancellationToken))
        {
            var permissions = new List<string>();
            var roles = await _userManager.GetRolesAsync(user);

            foreach (var role in roles)
            {
                var permissionList = await _context.RolePermissions
                    .Include(d => d.Permission)
                    .Where(x => x.Role.Name == role)
                    .Select(c => c.Permission.Name)
                    //.Select(p => p.Name)
                    .ToListAsync();

                permissions.AddRange(permissionList);
            }

            return permissions.Distinct().ToList();

        }
    }


}
