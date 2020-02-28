using Api3.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api3.Services
{
    public class IntrospectClient : IIntrospectClient
    {
        private readonly HttpClient httpClient;
        private readonly TokenIntrospectionRequest tokenIntrospectionRequest;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IntrospectClient(
            HttpClient httpClient,
            TokenIntrospectionRequest tokenIntrospectionRequest,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.tokenIntrospectionRequest = tokenIntrospectionRequest ?? throw new ArgumentNullException(nameof(tokenIntrospectionRequest));
        }

        public async Task<string> Get()
        {

            //don't like this
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            tokenIntrospectionRequest.Token = token;

            var tokenResponse = await httpClient.IntrospectTokenAsync(tokenIntrospectionRequest);

            if (tokenResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }
            return tokenResponse.Raw;
        }
    }
}
