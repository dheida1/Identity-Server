using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MvcClient.Web.DelegatingHandlers
{
    public class MtlsHandler : DelegatingHandler
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public MtlsHandler(IConfiguration configuration,
            IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var certificate = X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single();
            if (base.InnerHandler == null)
            {
                base.InnerHandler = new HttpClientHandler()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual,
                    PreAuthenticate = true,
                    AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                    CheckCertificateRevocationList = (environment.IsDevelopment() ? false : true),
                    SslProtocols = System.Security.Authentication.SslProtocols.Tls12
                };
            }
            var certBytes = certificate.RawData;
            var certString = Convert.ToBase64String(certBytes);
            request.Headers.Add("X-ARR-ClientCert", certString);


            return base.SendAsync(request, cancellationToken);
        }
    }
}
