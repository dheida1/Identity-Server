using MvcPkceClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Services
{
    public class Api1ClientService : IApi1Service
    {
        private readonly HttpClient client;

        public Api1ClientService(
            HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<string> Get()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/invoices/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
