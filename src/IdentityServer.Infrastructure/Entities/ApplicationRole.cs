using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public IList<RolePermission> RolePermissions { get; set; }
    }
}
