using IdentityModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace MvcMtlsClient.Web.DelegatingHandlers
{
    public class MtlsHandler : DelegatingHandler
    {
        private readonly IConfiguration configuration;
        private readonly IHostEnvironment environment;

        public MtlsHandler(IConfiguration configuration,
            IHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            X509Certificate2 certificate;
            if (!environment.IsDevelopment())
            {
                certificate = new Cryptography.X509Certificates.Extension.X509Certificate2(
                         configuration["Certificates:Personal"],
                         StoreName.My,
                         StoreLocation.LocalMachine,
                         X509FindType.FindBySerialNumber);
            }
            else
            {
                certificate = X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single();
            }
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
