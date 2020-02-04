using Api1.Interfaces;
using IdentityModel.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Api1.DelegatingHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IIdentityServerClient identityServerClient;

        public BearerTokenHandler(IIdentityServerClient identityServerClient)
        {
            this.identityServerClient = identityServerClient;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = identityServerClient.RequestClientCredentialsTokenAsync();
            request.SetBearerToken(accessToken.Result);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
