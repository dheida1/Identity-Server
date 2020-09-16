using MvcPkceClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Services
{
    public class Api1ClientService : IApi1ClientService
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
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                    throw new Exception(response.ReasonPhrase);
                }
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
