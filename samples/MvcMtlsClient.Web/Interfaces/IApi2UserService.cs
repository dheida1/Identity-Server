using System.Threading.Tasks;

namespace MvcMtlsClient.Web.Interfaces
{
    public interface IApi2UserService
    {
        Task<string> Get();
        Task<string> Delegate();
    }
}
