using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Api1.Features.Authorize
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
            // If user does not have the claim, get out of here
            if (!context.User.HasClaim(c => c.Type == configuration["IdentityServer:OtsPermissionsClaimType"] && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the claims string into an array           
            var permissionsClaim =
                context.User.FindFirst(c => c.Type == configuration["IdentityServer:OtsPermissionsClaimType"]
                && c.Issuer == requirement.Issuer).Value.Split(' ');

            // Succeed if the claim array contains the required claim
            if (permissionsClaim.Any(s => s == requirement.PermissionName))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
