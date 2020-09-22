using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Application { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public virtual IList<ApplicationRolePermission> RolePermissions { get; set; }
        public virtual IList<ApplicationProfileRole> ProfileRoles { get; set; }
    }
}
