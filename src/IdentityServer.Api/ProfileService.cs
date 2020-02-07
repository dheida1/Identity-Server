using IdentityServer.Core.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Api
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            this.userManager = userManager;
            this.claimsFactory = claimsFactory;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await userManager.FindByIdAsync(sub);
            var principal = await claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            //context.IssuedClaims = claims.ToList();

            //Add custom claims in token here based on user properties or any other source
            //claims.Add(new Claim("employee_id", user.EmployeeId ?? string.Empty));
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
