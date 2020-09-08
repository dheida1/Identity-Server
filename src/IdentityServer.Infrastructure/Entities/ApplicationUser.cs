using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Infrastructure.Entities
{
    public partial class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Locations { get; set; }
    }
}
