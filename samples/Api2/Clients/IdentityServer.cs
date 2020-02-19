using Api2.Interfaces;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api2.Clients
{
    public class IdentityServerClient : IIdentityServerClient
    {
        private readonly HttpClient httpClient;
        private readonly TokenRequest tokenRequest;
        private readonly IHttpContextAccessor httpContextAccessor;

        public IdentityServerClient(
            HttpClient httpClient,
            TokenRequest tokenRequest,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            this.tokenRequest = tokenRequest ?? throw new ArgumentNullException(nameof(tokenRequest));
        }

        public async Task<string> RequestTokenAsync()
        {
            //don't like this
            var token = await httpContextAccessor.HttpContext.GetTokenAsync("access_token");
            tokenRequest.Parameters.Remove("token");
            tokenRequest.Parameters.Add("token", token);

            // request the access token token
            var tokenResponse = await httpClient.RequestTokenAsync(tokenRequest);
            if (tokenResponse.IsError)
            {
                throw new HttpRequestException("Something went wrong while requesting the access token");
            }
            return tokenResponse.AccessToken;
        }
    }
}