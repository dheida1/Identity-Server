using MvcClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcClient.Web.Services
{
    public class Api2ServiceClient : IApi2ServiceClient
    {
        private readonly HttpClient client;

        public Api2ServiceClient(
            HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> Get()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/api2/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
