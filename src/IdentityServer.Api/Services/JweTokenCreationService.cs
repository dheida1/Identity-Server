using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace IdentityServer.Api.Services
{
    public class JweTokenCreationService : DefaultTokenCreationService
    {

        private readonly IClientStore clientStore;

        public JweTokenCreationService(
           IClientStore clientStore,
           ISystemClock clock,
           IKeyMaterialService keys,
           IdentityServerOptions options,
           ILogger<DefaultTokenCreationService> logger)
           : base(clock, keys, options, logger)
        {
            this.clientStore = clientStore;
        }

        public override async Task<string> CreateTokenAsync(Token token)
        {
            //if (token.Type == TokenTypes.IdentityToken)
            //{
            //    var client = await clientStore.FindEnabledClientByIdAsync(token.ClientId);
            //    var clientRawCertificate = client.Properties.GetOrDefault("RequireJwe");

            //    var clientCertificate = new X509Certificate2(Convert.FromBase64String(clientRawCertificate));
            //    var payload = await base.CreatePayloadAsync(token);

            //    var handler = new JsonWebTokenHandler();
            //    var jwe = handler.CreateToken(
            //        payload.SerializeToJson(),
            //        await Keys.GetSigningCredentialsAsync(),
            //        new X509EncryptingCredentials(clientCertificate));

            //    return jwe;
            //}
            return await base.CreateTokenAsync(token);
        }
    }
}
