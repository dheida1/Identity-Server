using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationProfileRole
    {
        public Guid RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual ApplicationRole Role { get; set; }

        public Guid ProfileId { get; set; }

        [ForeignKey("ProfileId")]
        public virtual ApplicationProfile Proflle { get; set; }

    }
}
