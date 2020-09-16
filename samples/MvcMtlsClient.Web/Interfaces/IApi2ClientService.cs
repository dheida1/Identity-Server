using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Interfaces
{
    public interface IApi2ClientService
    {
        Task<string> Get();
        Task<string> Delegate();
    }
}
