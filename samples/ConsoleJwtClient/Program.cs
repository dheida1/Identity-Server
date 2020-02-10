using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace ConsoleJwtClient
{
    public class Program
    {
        public static async Task Main()
        {
            Console.Title = "Console Client Credentials Flow with JWT Assertion";

            var response = await RequestTokenAsync();
            Console.WriteLine(response.Json);

            Console.ReadLine();
            await CallServiceAsync(response.AccessToken);
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await client.GetDiscoveryDocumentAsync//("https://localhost:4300");

           (new DiscoveryDocumentRequest()
           { Address = "https://localhost:4300", Policy = new DiscoveryPolicy() { RequireHttps = false, Authority = "https://localhost:4300" } });

            if (disco.IsError) throw new Exception(disco.Error);

            var clientToken = CreateClientToken("client.jwt", disco.TokenEndpoint);

            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client.jwt",
                Scope = "api2",

                ClientAssertion =
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = clientToken
                }
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        static async Task CallServiceAsync(string token)
        {
            var baseAddress = "https://localhost:5003";

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            client.SetBearerToken(token);
            var response = await client.GetStringAsync("api2/ApiSecure");
            Console.WriteLine(JArray.Parse(response));
        }

        private static string CreateClientToken(string clientId, string audience)
        {
            //uncomment and correct to absolute filepath if searching for cert from folder.
            //var filePath = @"C:\Users\dinah\source\repos\IdentityServer\samples\ConsoleJwtClient\Certificates\ClientJwt.pfx";
            //var certificate = new X509Certificate2(filePath, "1234", X509KeyStorageFlags.Exportable);

            //if searching for cert from 'Local Machine' Personal Store 
            var certificate = new Cryptography.X509Certificates.Extension.X509Certificate2("2767798A6DC7691C8EF41414BF7C9D59DB9DA31A",
                StoreName.My, StoreLocation.LocalMachine, X509FindType.FindByThumbprint, false);

            var now = DateTime.UtcNow;

            var token = new JwtSecurityToken(
                    clientId,
                    audience,
                    new List<Claim>()
                    {
                        new Claim("jti", Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.Subject, clientId),
                        new Claim(JwtClaimTypes.IssuedAt, now.ToEpochTime().ToString(), ClaimValueTypes.Integer64)
                    },
                    now,
                    now.AddMinutes(1),
                    new SigningCredentials(
                        new X509SecurityKey(certificate),
                        SecurityAlgorithms.RsaSha256
                    )
                );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
