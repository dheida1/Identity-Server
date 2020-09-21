using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string FriendlyName { get; set; }
        public virtual IList<ApplicationRolePermission> RolePermissions { get; set; }
    }
}
