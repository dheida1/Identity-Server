using IdentityModel;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;

namespace MvcMtlsClient.Web.DelegatingHandlers
{
    public static class MtlsSocketHandler
    {
        //private readonly IConfiguration configuration;

        static SocketsHttpHandler GetMtlsHandler(this HttpClient httpClient, IConfiguration configuration)
        {
            var handler = new SocketsHttpHandler();

            var certificate = X509.LocalMachine.My.Thumbprint.Find(configuration["Certificates:Personal"], false).Single();
            handler.SslOptions.ClientCertificates = new X509CertificateCollection { certificate };

            return handler;
        }
    }
}
