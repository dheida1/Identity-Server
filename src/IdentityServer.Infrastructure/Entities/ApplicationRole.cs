using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer.Infrastructure.Entities
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string FriendlyName { get; set; }
    }
}
