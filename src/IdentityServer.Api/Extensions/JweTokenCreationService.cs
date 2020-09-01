using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace IdentityServer.Api.Extensions
{
    public class JweTokenCreationService : DefaultTokenCreationService
    {

        private readonly IClientStore extClientStore;

        public JweTokenCreationService(
           IClientStore extClientStore,
           ISystemClock clock,
           IKeyMaterialService keys,
           IdentityServerOptions options,
           ILogger<DefaultTokenCreationService> logger)
           : base(clock, keys, options, logger)
        {
            this.extClientStore = extClientStore;
        }

        public override async Task<string> CreateTokenAsync(Token token)
        {
            if (token.Type == TokenTypes.IdentityToken)
            {
                //TODO must extend ClientStore
                var clientCertificate = new X509Certificate2(); //await extClientStore.FindClientCertificate(token.ClientId);
                var payload = await base.CreatePayloadAsync(token);

                var handler = new JsonWebTokenHandler();
                var jwe = handler.CreateToken(
                    payload.SerializeToJson(),
                    await Keys.GetSigningCredentialsAsync(),
                    new X509EncryptingCredentials(clientCertificate));

                return jwe;
            }
            return await base.CreateTokenAsync(token);
        }
    }
}
