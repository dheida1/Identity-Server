using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Interfaces
{
    public interface IApi1ServiceClient
    {
        Task<string> Get();
    }
}
