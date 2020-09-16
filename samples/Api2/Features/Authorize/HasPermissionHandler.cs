using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Api2.Features.Authorize
{
    public class HasPermissionHandler : AuthorizationHandler<HasPermissionRequirement>
    {
        private readonly IConfiguration configuration;

        public HasPermissionHandler(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasPermissionRequirement requirement)
        {
            string claimType = (context.User.Claims.Any(c => c.Type == JwtClaimTypes.PreferredUserName) ?
                    configuration["IdentityServer:OtsPermissionsClaimType"] : "scope");

            // If user does not have the claim, get out of here
            if (!context.User.HasClaim(c => c.Type == claimType && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the claims string into an array           
            var permissionsClaim =
                context.User.FindAll(c => c.Type == claimType
                && c.Issuer == requirement.Issuer).Select(x => x.Value).ToArray();

            // Succeed if the claim array contains the required claim
            if (permissionsClaim.Intersect(requirement.PermissionName.Split(' ')).Any())
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
