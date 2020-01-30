using MvcClient.Web.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MvcClient.Web.Services
{
    public class Api1Service : IApi1Service
    {
        private readonly HttpClient client;

        public Api1Service(
            HttpClient client)
        {
            this.client = client ?? throw new ArgumentNullException(nameof(client));
        }

        //public Api1Service(HttpClient httpClient,
        //    [FromServices] IHttpContextAccessor context,
        //    [FromServices] IConfiguration configuration,
        //    [FromServices] IJwes jwes)
        //{
        //    string apiEncryptingCertificateSerialNumber = configuration["JWT:Api1EncryptingCertificateSerialNumber"];
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwes.GetJWES(context.HttpContext.User.Claims, apiEncryptingCertificateSerialNumber));
        //    this.httpClient = httpClient;
        //}


        public async Task<string> Get()
        {
            // No more getting access_tokens code!
            var response = await client.GetAsync("/api1/ApiSecure");

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
                throw new Exception("Failed to get protected resources.");
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
