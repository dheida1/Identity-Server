//using IdentityModel.Client;
//using Microsoft.Extensions.Logging;
//using MvcClient.Web.Interfaces;
//using System;
//using System.Net.Http;
//using System.Threading.Tasks;

//namespace MvcClient.Web.Services
//{
//    public class IdentityServerClient : IIdentityServerClient
//    {
//        private readonly HttpClient httpClient;
//        //private readonly ClientCredentialsTokenRequest tokenRequest;
//        private readonly TokenRequest tokenRequest;
//        private readonly ILogger<IdentityServerClient> logger;

//        public IdentityServerClient(
//            HttpClient httpClient,
//            //ClientCredentialsTokenRequest tokenRequest,
//            TokenRequest tokenRequest,
//            ILogger<IdentityServerClient> logger)
//        {
//            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
//            this.tokenRequest = tokenRequest ?? throw new ArgumentNullException(nameof(tokenRequest));
//            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
//        }

//        public async Task<string> RequestClientCredentialsTokenAsync()
//        {
//            // request the access token token
//            var tokenResponse = await httpClient.RequestTokenAsync(tokenRequest);
//            //var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(tokenRequest);
//            if (tokenResponse.IsError)
//            {
//                logger.LogError(tokenResponse.Error);
//                throw new HttpRequestException("Something went wrong while requesting the access token");
//            }
//            return tokenResponse.AccessToken;
//        }
//    }
//}