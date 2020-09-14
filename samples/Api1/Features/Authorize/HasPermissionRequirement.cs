using Microsoft.AspNetCore.Authorization;
using System;

namespace Api1.Features.Authorize
{
    public class HasPermissionRequirement : IAuthorizationRequirement
    {
        public HasPermissionRequirement(string permissionName, string issuer)
        {
            PermissionName = permissionName ?? throw new ArgumentNullException(nameof(permissionName));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }

        public string PermissionName { get; }
        public string Issuer { get; }
    }
}
