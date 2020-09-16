using MvcMtlsClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Services
{
    public class Api2UserService : IApi2UserService
    {
        private readonly HttpClient client;

        public Api2UserService(
            HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> Get()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/inventory/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                Console.WriteLine(response.ReasonPhrase);
                throw new Exception(response.ReasonPhrase);
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> Delegate()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/invoices/ApiDelegate");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
