using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MvcClient.Web.Services
{
    public class Api1Service : IApi1Service
    {
        public HttpClient client { get; private set; }

        public Api1Service(HttpClient httpClient,
            [FromServices] IHttpContextAccessor context,
            [FromServices] IConfiguration configuration,
            [FromServices] IJwes jwes)
        {
            string apiEncryptingCertificateSerialNumber = configuration["JWT:Api1EncryptingCertificateSerialNumber"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwes.GetJWES(context.HttpContext.User.Claims, apiEncryptingCertificateSerialNumber));
            client = httpClient;
        }

        public async Task<string> GetValues()
        {
            var response = await client.GetStringAsync("/api1/values/get");
            return response;
        }
    }
}
