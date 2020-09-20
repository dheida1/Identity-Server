using Identity.ExtensionStore.IdentityPermission;
using System;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationPermission : IdentityPermission<Guid>
    {
        public string FriendlyName { get; set; }
        public string Application { get; set; }
    }
}
