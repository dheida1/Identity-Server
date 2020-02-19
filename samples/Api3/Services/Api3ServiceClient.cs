using Api3.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Api3.Services
{
    public class Api3ServiceClient : IApi3ServiceClient
    {
        private readonly HttpClient client;

        public Api3ServiceClient(
            HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> Get()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/api3/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
            }
            return await response.Content.ReadAsStringAsync();
        }

    }
}
