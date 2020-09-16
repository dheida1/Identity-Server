using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Polly;
using Serilog;
using Serilog.Core;
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
        public static class Globals
        {
            public static Logger logger;
            static Globals()
            {
                logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .Enrich.FromLogContext()
                         .WriteTo.ColoredConsole()
                         .CreateLogger();


            }
        }

        public static async Task Main()
        {
            Console.Title = "Console Client Credentials Flow with JWT Assertion";

            var response = await RequestTokenAsync();
            Console.WriteLine("{0} {1}", "Access Token", response.Json);

            Console.WriteLine("Call the Invoice Api Service....");
            await CallServiceAsync(response.AccessToken);
            Console.ReadLine();
        }

        static async Task<TokenResponse> RequestTokenAsync()
        {
            var client = new HttpClient();

            var disco = await Policy
                           .HandleResult<DiscoveryDocumentResponse>(message => message.IsError)
                           .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
                           {
                               Globals.logger.Warning($"Request failed with {result.Result.Error}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                           })
                           .ExecuteAsync(async () =>
                           {
                               return await client.GetDiscoveryDocumentAsync(
                                    new DiscoveryDocumentRequest()
                                    {
                                        Address = "https://localhost:4300",
                                        Policy = new DiscoveryPolicy()
                                        {
                                            RequireHttps = false,
                                            Authority = "https://localhost:4300"
                                        }
                                    });
                           });

            if (disco.IsError) throw new Exception(disco.Error);

            var clientToken = CreateClientToken("client.jwt", disco.TokenEndpoint);

            Console.WriteLine("Create request token");
            Console.WriteLine("{0} {1}", "Client token request:", clientToken);


            Console.WriteLine("Requesting token from IdentityServer");
            var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "client.jwt",
                Scope = "invoices.read",

                ClientAssertion =
                {
                    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    Value = clientToken
                }
            });

            if (response.IsError) throw new Exception(response.Error);
            return response;
        }

        //call the Invoices api (api1)
        static async Task CallServiceAsync(string token)
        {
            var baseAddress = "https://localhost:5002";

            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };

            var response = await Policy
                            .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
                            .WaitAndRetryAsync(3, i => TimeSpan.FromSeconds(2), (result, timeSpan, retryCount, context) =>
                            {
                                Globals.logger.Warning($"Request failed with {result.Result.StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}");
                            })
                            //.ExecuteAsync(() =>
                            .ExecuteAsync(async () =>
                            {
                                client.SetBearerToken(token);
                                return await client.GetAsync("invoices/ApiSecure");
                            });


            Console.WriteLine("Invoice Api Service response");
            Console.WriteLine(await response.Content.ReadAsStringAsync());
        }

        private static string CreateClientToken(string clientId, string audience)
        {
            //Dev environment
            //uncomment and correct to absolute filepath if searching for cert from folder.
            var filePath = @"C:\Users\dinah\source\repos\IdentityServer\samples\ConsoleJwtClient\Certificates\ClientJwt.pfx";
            var certificate = new X509Certificate2(filePath, "1234", X509KeyStorageFlags.Exportable);

            //Non Prod environment
            //if searching for cert from 'Local Machine' Personal Store
            //var certificate = new Cryptography.X509Certificates.Extension.X509Certificate2(
            //    @"E=InfoSecTeam@la.gov, CN=Mvc Web Client, OU=OTS, O=State of Louisiana, L=Baton Rouge, S=Louisiana, C=US",
            //    StoreName.My,
            //    StoreLocation.LocalMachine,
            //    X509FindType.FindBySubjectDistinguishedName,
            //    false);

            var now = DateTime.UtcNow;

            //signed JWT (JWS)
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
