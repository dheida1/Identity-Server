using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Interfaces
{
    public interface IApi2ServiceClient
    {
        Task<string> Get();
    }
}
