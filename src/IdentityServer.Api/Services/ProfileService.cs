using Identity.ExtensionStore.IdentityPermission;
using IdentityServer.Infrastructure.Entities;
using IdentityServer.Infrastructure.Interfaces;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Api.Services
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPermissionStore<ApplicationUser> permissionStore;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory;
        private readonly IConfiguration configuration;
        private readonly IRolePermissionStore<ApplicationUser, ApplicationRole>

        public ProfileService(
            UserManager<ApplicationUser> userManager,
            IPermissionStore<ApplicationUser> permissionStore,
            IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.claimsFactory = claimsFactory;
            this.permissionStore = permissionStore;
            this.configuration = configuration;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();

            var user = await userManager.FindByIdAsync(sub);
            var principal = await claimsFactory.CreateAsync(user);

            var claims = principal.Claims.ToList();
            claims = claims.Where(claim => context.RequestedClaimTypes.Contains(claim.Type)).ToList();

            if (context.RequestedClaimTypes.Contains(configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"]))
            {
                var permissions = await permissionStore.GetUserPermissions(user);
                foreach (var permission in permissions)
                {
                    claims.Add(new Claim(configuration["AppConfiguration:AgencyConfiguration:OtsPermissionsClaimType"],
                        permission,
                        ClaimValueTypes.String,
                        configuration["AppConfiguration:IdentityServer:Authority"]
                        ));
                }
            }
            context.AddRequestedClaims(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
