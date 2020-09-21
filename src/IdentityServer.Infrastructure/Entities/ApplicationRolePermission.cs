using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationRolePermission
    {
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }

        public Guid PermissionId { get; set; }

        [ForeignKey("PermissionId")]
        public virtual ApplicationPermission Permission { get; set; }
    }
}
