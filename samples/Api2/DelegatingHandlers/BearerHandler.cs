//using Api2.Interfaces;
//using IdentityModel.Client;
//using System.Net.Http;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Api2.DelegatingHandlers
//{
//    public class BearerHandler : DelegatingHandler
//    {
//        private readonly IIdentityServerClient identityServerClient;

//        public BearerHandler(
//            IIdentityServerClient identityServerClient)
//        {
//            this.identityServerClient = identityServerClient;
//        }
//        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            // request the access token
//            var accessToken = await identityServerClient.RequestTokenAsync();

//            // set the bearer token to the outgoing request
//            request.SetBearerToken(accessToken);

//            // Proceed calling the inner handler, that will actually send the request
//            // to our protected api
//            return await base.SendAsync(request, cancellationToken);
//        }
//    }
//}