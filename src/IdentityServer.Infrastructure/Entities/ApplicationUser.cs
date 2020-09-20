using Microsoft.AspNetCore.Identity;
using System;

namespace IdentityServer.Infrastructure.Entities
{
    public partial class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locations { get; set; }
    }
}
