using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MvcClient.Web.DelegatingHandlers
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor context;

        public BearerTokenHandler(IHttpContextAccessor context)
        {
            this.context = context;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var access_token = context.HttpContext.GetTokenAsync("access_token");
            request.SetBearerToken(access_token.Result);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
