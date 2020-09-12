using IdentityServer.Infrastructure.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserPermissionStore<ApplicationUser> userPermissionStore;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory;

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IUserPermissionStore<ApplicationUser> userPermissionStore,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
        {
            this.userManager = userManager;
            this.claimsFactory = claimsFactory;
            this.userPermissionStore = userPermissionStore;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await userManager.FindByIdAsync(sub);
            var principal = await claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            var permissions = await userPermissionStore.GetUserPermissions(user);
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("ots-permissions", permission));
            }


            //claims.AddRange()

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
