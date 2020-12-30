using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Configurations
{
    public class CookieEventHandler : CookieAuthenticationEvents
    {
        public CookieEventHandler(LogoutSessionManager logoutSessions)
        {
            LogoutSessions = logoutSessions;
        }

        public LogoutSessionManager LogoutSessions { get; }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (context.Principal.Identity.IsAuthenticated)
            {
                var sub = context.Principal.FindFirst("sub")?.Value;
                var sid = context.Principal.FindFirst("sid")?.Value;

                if (await LogoutSessions.IsLoggedOutAsync(sub, sid))
                {
                    context.RejectPrincipal();
                    await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                }
            }
        }
        public override async Task SigningOut(CookieSigningOutContext context)
        {
            // automatically revoke refresh token at signout time
            await context.HttpContext.RevokeUserRefreshTokenAsync();
        }
    }
}
