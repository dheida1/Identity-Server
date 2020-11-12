using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer.Infrastructure.Entities
{
    public partial class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser() : base() { }
        public ApplicationUser(string name) : base(name) { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
