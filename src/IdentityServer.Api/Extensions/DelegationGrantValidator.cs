using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Api.Extensions
{
    public class DelegationGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator tokenValidator;

        public DelegationGrantValidator(ITokenValidator tokenValidator)
        {
            this.tokenValidator = tokenValidator;
        }

        public string GrantType => "delegation";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var accessToken = context.Request.Raw.Get("token");

            if (string.IsNullOrEmpty(accessToken))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            //get id_token that belongs to this user
            var result = await tokenValidator.ValidateAccessTokenAsync(accessToken);
            if (result.IsError)
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            // get user's identity
            var sub = result.Claims.FirstOrDefault(c => c.Type == "sub").Value;

            context.Result = new GrantValidationResult(sub, GrantType, result.Claims);
            return;
        }
    }
}
