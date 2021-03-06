﻿using MvcPkceClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcPkceClient.Web.Services
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
            var response = await client.GetAsync("/invoices/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
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
