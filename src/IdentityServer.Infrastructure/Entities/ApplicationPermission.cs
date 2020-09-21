using System;
using System.Collections.Generic;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationPermission
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Application { get; set; }
        public virtual IList<ApplicationRolePermission> RolePermissions { get; set; }
    }
}
