using System;

namespace IdentityServer.Infrastructure.Entities
{
    public class RolePermission
    {
        public Guid Id { get; set; }
        public string RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }
        public Guid PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
