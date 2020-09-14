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
            string claimType = (context.User.Identity.Name != null ? configuration["IdentityServer:OtsPermissionsClaimType"] : "scope");


            //if (context.User.Identity.Name != null) //real users
            //{
            // If user does not have the claim, get out of here
            if (!context.User.HasClaim(c => c.Type == configuration["IdentityServer:OtsPermissionsClaimType"] && c.Issuer == requirement.Issuer))
                return Task.CompletedTask;

            // Split the claims string into an array           
            var permissionsClaim =
                context.User.FindAll(c => c.Type == configuration["IdentityServer:OtsPermissionsClaimType"]
                && c.Issuer == requirement.Issuer).Select(x => x.Value).ToArray();

            // Succeed if the claim array contains the required claim
            if (permissionsClaim.Intersect(requirement.PermissionName.Split(' ')).Any())
                context.Succeed(requirement);
            //}
            //else //jobs and client console apps
            //{
            //    // If user does not have the scope claim, get out of here
            //    if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == requirement.Issuer))
            //        return Task.CompletedTask;

            //    // Split the scopes string into an array
            //    var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == requirement.Issuer).Value.Split(' ');

            //    // Succeed if the scope array contains the required scope
            //    if (scopes.Any(s => s == requirement.PermissionName))
            //        context.Succeed(requirement);

            //    return Task.CompletedTask;
            //}
            return Task.CompletedTask;
        }
    }
}
